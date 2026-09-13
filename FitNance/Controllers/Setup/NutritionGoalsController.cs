using FitNance.Data;
using FitNance.Models.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitNance.Data;
using FitNance.Models.Setup;
using Microsoft.AspNetCore.Authorization;

namespace FitNance.Controllers.Setup
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionGoalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NutritionGoalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNutritionGoals()
        {
            var nutritionGoals = await _context.Set<NutritionGoalsModel>().FromSqlRaw(@"SELECT GoalId,GoalName,Description FROM Setup_NutritionGoals").ToListAsync();

            return Ok(nutritionGoals);
        }
    }
}
