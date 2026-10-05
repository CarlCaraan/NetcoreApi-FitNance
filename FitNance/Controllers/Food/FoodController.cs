using System.Security.Claims;
using FitNance.Data;
using FitNance.Models.Food;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Controllers.Food
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FoodController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetFoods(
          int pageNumber = 1,
          int pageSize = 30,
          string? search = null,
          string source = "all")
        {
            // ==========================================
            // GET USERNAME FROM JWT
            // ==========================================
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify the logged-in user."
                });
            }

            // ==========================================
            // VALIDATE PAGINATION
            // ==========================================
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1 || pageSize > 100)
                pageSize = 30;

            // ==========================================
            // GET FOOD QUERY
            // ==========================================
            var query = _context.MasterFoods
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // FOOD SOURCE FILTER
            // ==========================================
            if (source == "system")
            {
                // System Default Foods
                query = query.Where(x => x.UserId == "D");
            }
            else if (source == "user")
            {
                // Foods created by logged-in user
                query = query.Where(x => x.UserId == userId);
            }
            else
            {
                // All Foods
                query = query.Where(x => x.UserId == "D" || x.UserId == userId);
            }

            // ==========================================
            // SEARCH
            // ==========================================
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FoodName.Contains(search));
            }

            // ==========================================
            // TOTAL COUNT
            // ==========================================
            var totalCount = await query.CountAsync();

            // ==========================================
            // PAGINATION
            // ==========================================
            var items = await query
                .OrderBy(x => x.FoodName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================
            return Ok(new FoodListResponse
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFood([FromBody] MasterFoodModel food)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.Name)?.Value; //username

            var lastFoodId = await _context.MasterFoods
                .Where(x => x.FoodId != null && x.FoodId.StartsWith("FD"))
                .OrderByDescending(x => x.FoodId)
                .Select(x => x.FoodId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastFoodId) && int.TryParse(lastFoodId.Substring(2), out int lastNumber))
                nextNumber = lastNumber + 1;

            food.FoodId = $"FD{nextNumber:D6}";
            food.CreatedBy = userId;
            food.CreatedDate = DateTime.Now;
            food.ModifiedBy = null;
            food.ModifiedDate = null;
            food.UserId = userId;

            _context.MasterFoods.Add(food);
            await _context.SaveChangesAsync();

            return Ok(food);

        }


        [HttpPut("{foodId}")]
        [Authorize]
        public async Task<IActionResult> UpdateFood(string foodId, [FromBody] MasterFoodModel food)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            // ==========================================
            // FIND EXISTING FOOD
            // ==========================================
            var existingFood = await _context.MasterFoods
                .FirstOrDefaultAsync(x => x.FoodId == foodId && x.UserId == userId);

            if (existingFood == null)
                return NotFound(new { message = "Food not found." });

            // ==========================================
            // UPDATE FOOD
            // ==========================================
            existingFood.FoodName = food.FoodName;
            existingFood.Category = food.Category;
            existingFood.ServingSize = food.ServingSize;
            existingFood.ServingUnit = food.ServingUnit;
            existingFood.ServingGrams = food.ServingGrams;
            existingFood.Calories = food.Calories;
            existingFood.Protein = food.Protein;
            existingFood.Carbs = food.Carbs;
            existingFood.Fat = food.Fat;
            existingFood.Fiber = food.Fiber;
            existingFood.Sodium = food.Sodium;
            existingFood.Sugar = food.Sugar;
            existingFood.Cholesterol = food.Cholesterol;
            existingFood.IsCanned = food.IsCanned;
            existingFood.IsFastFood = food.IsFastFood;

            // ==========================================
            // UPDATE AUDIT FIELDS
            // ==========================================
            existingFood.ModifiedBy = userId;
            existingFood.ModifiedDate = DateTime.Now;

            // ==========================================
            // SAVE CHANGES
            // ==========================================
            await _context.SaveChangesAsync();

            return Ok(existingFood);
        }



        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteFood([FromBody] List<string> foodIds)
        {
            if (foodIds == null || foodIds.Count == 0)
                return BadRequest("No food selected.");

            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            var foods = await _context.MasterFoods
                .Where(x => foodIds.Contains(x.FoodId) && x.UserId == userId)
                .ToListAsync();

            if (foods.Count == 0)
                return NotFound("No user-owned foods found.");

            _context.MasterFoods.RemoveRange(foods);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{foods.Count} food(s) deleted successfully.",
                deletedCount = foods.Count
            });
        }




    }
}
