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
        public async Task<IActionResult> GetFoods(int pageNumber = 1, int pageSize = 30, string? search = null)
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
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 30;

            // ==========================================
            // GET DEFAULT + USER FOODS
            // ==========================================
            var query = _context.MasterFoods
                .AsNoTracking()
                .Where(x => x.UserId == "D" || x.UserId == userId);

            // ==========================================
            // SEARCH
            // ==========================================
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.FoodName.Contains(search));
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
    }
}
