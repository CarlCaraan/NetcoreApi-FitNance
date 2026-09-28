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
    public class FoodCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodCategory()
        {
            var foodCategory = await _context.Set<FoodCategoryModel>().FromSqlRaw(@"SELECT CategoryId,CategoryName,Description FROM Setup_FoodCategory").ToListAsync();

            return Ok(foodCategory);
        }
    }
}
