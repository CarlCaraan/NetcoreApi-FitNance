using System.Data;
using FitNance.Data;
using FitNance.Models.MealPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Controllers.MealPlan;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealPlanController(AppDbContext context) : ControllerBase
{
    private string? Username => User.Identity?.Name;

    private IQueryable<MealPlanHeader> OwnedPlans =>
        context.MealPlans.Where(plan => plan.UserProfileId == Username);

    [HttpGet]
    public async Task<IActionResult> List(string? search, int pageNumber = 1, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(Username))
            return Unauthorized();

        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = OwnedPlans.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(plan => plan.PlanName.Contains(search.Trim()));

        var totalCount = await query.CountAsync();
        pageNumber = Math.Clamp(pageNumber, 1, Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize)));
        var items = await query.OrderBy(plan => plan.PlanName).ThenBy(plan => plan.RowStamp)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .Select(plan => new
            {
                plan.MealPlanId, plan.PlanName, plan.StartDate, plan.EndDate,
                plan.TotalCalories, plan.TotalProtein, plan.TotalCarbs, plan.TotalFat,
                ItemCount = plan.Details.Count
            }).ToListAsync();

        return Ok(new { items, totalCount, pageNumber, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        if (string.IsNullOrWhiteSpace(Username))
            return Unauthorized();

        var plan = await OwnedPlans.AsNoTracking().Include(plan => plan.Details)
            .SingleOrDefaultAsync(plan => plan.MealPlanId == id);
        if (plan is null)
            return NotFound();

        var foodIds = plan.Details.Select(detail => detail.FoodId).Distinct().ToArray();
        var foods = await context.MasterFoods.Where(food => foodIds.Contains(food.FoodId!)
                && (food.UserId == "D" || food.UserId == Username))
            .Select(food => new { food.FoodId, food.FoodName }).ToListAsync();
        var foodNames = foods.ToDictionary(food => food.FoodId!.Trim(), food => food.FoodName,
            StringComparer.OrdinalIgnoreCase);

        return Ok(new
        {
            plan.MealPlanId, plan.PlanName, plan.StartDate, plan.EndDate,
            plan.TotalCalories, plan.TotalProtein, plan.TotalCarbs, plan.TotalFat, plan.TotalFiber,
            Details = plan.Details.OrderBy(detail => detail.MealDate).ThenBy(detail => detail.RowStamp)
                .Select(detail => new
                {
                    detail.MealPlanDetailId, detail.FoodId, detail.MealType, detail.MealDate,
                    detail.Quantity, detail.QuantityGrams, detail.Calories, detail.Protein,
                    detail.Carbs, detail.Fat, detail.Fiber, detail.Sugar, detail.Sodium,
                    FoodName = foodNames.GetValueOrDefault(detail.FoodId.Trim()) ?? detail.FoodId
                })
        });
    }

    [HttpPost]
    public Task<IActionResult> Create(MealPlanRequest request) => Save(null, request);

    [HttpPut("{id}")]
    public Task<IActionResult> Update(string id, MealPlanRequest request) => Save(id, request);

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(Username))
            return Unauthorized();

        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var plan = await OwnedPlans.Include(plan => plan.Details)
            .SingleOrDefaultAsync(plan => plan.MealPlanId == id);
        if (plan is null)
            return NotFound();

        context.MealPlanDetails.RemoveRange(plan.Details);
        context.MealPlans.Remove(plan);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return NoContent();
    }

    private async Task<IActionResult> Save(string? id, MealPlanRequest request)
    {
        var username = Username;
        if (string.IsNullOrWhiteSpace(username))
            return Unauthorized();
        if (request.EndDate < request.StartDate)
            return BadRequest(new { message = "End date must be on or after the start date." });
        if (request.Details.Any(detail => detail is null || detail.MealDate < request.StartDate
                || detail.MealDate > request.EndDate))
            return BadRequest(new { message = "Each meal date must fall within the plan dates." });

        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var now = DateTime.Now;
        var plan = id is null
            ? new MealPlanHeader
            {
                MealPlanId = NewId(), UserProfileId = username, CreatedBy = username, CreatedDate = now
            }
            : await OwnedPlans.Include(plan => plan.Details).SingleOrDefaultAsync(plan => plan.MealPlanId == id);
        if (plan is null)
            return NotFound();

        var existing = plan.Details.ToDictionary(detail => detail.MealPlanDetailId);
        var foodIds = request.Details.Select(detail => detail.FoodId.Trim()).Distinct().ToArray();
        var foodRows = await context.MasterFoods.Where(food => foodIds.Contains(food.FoodId!)
            && (food.UserId == "D" || food.UserId == username)).ToListAsync();
        var foods = foodRows.ToDictionary(food => food.FoodId!.Trim(), StringComparer.OrdinalIgnoreCase);
        var details = new List<MealPlanDetail>();

        foreach (var row in request.Details)
        {
            MealPlanDetail detail;
            if (row.MealPlanDetailId is null)
            {
                detail = new MealPlanDetail
                {
                    MealPlanDetailId = NewId(), MealPlanId = plan.MealPlanId,
                    CreatedBy = username, CreatedDate = now
                };
            }
            else
            {
                if (!existing.Remove(row.MealPlanDetailId, out var savedDetail))
                    return BadRequest(new { message = "Invalid or repeated meal plan item." });
                detail = savedDetail;
                if (detail.MealType != row.MealType.Trim() || detail.MealDate != row.MealDate
                    || detail.Quantity != row.Quantity || !SameFood(detail.FoodId, row.FoodId))
                {
                    detail.ModifiedBy = username;
                    detail.ModifiedDate = now;
                }
            }

            if (row.MealPlanDetailId is null || !SameFood(detail.FoodId, row.FoodId)
                || detail.Quantity != row.Quantity)
            {
                if (!foods.TryGetValue(row.FoodId.Trim(), out var food) || row.Quantity is null)
                    return BadRequest(new { message = "Choose an available food and enter a quantity for each new or changed serving." });

                detail.FoodId = food.FoodId!.Trim();
                detail.Quantity = row.Quantity;
                detail.QuantityGrams = food.ServingGrams * row.Quantity;
                detail.Calories = food.Calories * row.Quantity;
                detail.Protein = food.Protein * row.Quantity;
                detail.Carbs = food.Carbs * row.Quantity;
                detail.Fat = food.Fat * row.Quantity;
                detail.Fiber = food.Fiber * row.Quantity;
                detail.Sugar = food.Sugar * row.Quantity;
                detail.Sodium = food.Sodium * row.Quantity;
            }

            detail.MealType = row.MealType.Trim();
            detail.MealDate = row.MealDate;
            details.Add(detail);
        }

        context.MealPlanDetails.RemoveRange(existing.Values);
        plan.Details = details;
        plan.PlanName = request.PlanName.Trim();
        plan.StartDate = request.StartDate;
        plan.EndDate = request.EndDate;
        plan.TotalCalories = details.Sum(detail => detail.Calories);
        plan.TotalProtein = details.Sum(detail => detail.Protein);
        plan.TotalCarbs = details.Sum(detail => detail.Carbs);
        plan.TotalFat = details.Sum(detail => detail.Fat);
        plan.TotalFiber = details.Sum(detail => detail.Fiber);

        if (id is null)
            context.MealPlans.Add(plan);
        else
        {
            plan.ModifiedBy = username;
            plan.ModifiedDate = now;
            context.MealPlanDetails.AddRange(details.Where(detail => detail.RowStamp == 0));
        }

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return id is null
            ? CreatedAtAction(nameof(Get), new { id = plan.MealPlanId }, new { plan.MealPlanId })
            : NoContent();
    }

    private static bool SameFood(string first, string second) =>
        string.Equals(first.Trim(), second.Trim(), StringComparison.OrdinalIgnoreCase);

    private static string NewId() => Guid.NewGuid().ToString("N")[..30];
}
