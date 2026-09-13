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
    public class ActivityLevelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivityLevelController(AppDbContext context) 
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetActivityLevels()
        {
            var activityLevels = await _context.Set<ActivityLevelModel>().FromSqlRaw(@"SELECT ActivityLevelId, ActivityLevelName, Description FROM Setup_ActivityLevel").ToListAsync();

            return Ok(activityLevels);
        }









    }
}
