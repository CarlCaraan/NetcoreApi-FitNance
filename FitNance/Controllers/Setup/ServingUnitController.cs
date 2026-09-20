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
        public class ServingUnitController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ServingUnitController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<IActionResult> GetServingUnit()
            {
                var servingUnit = await _context.Set<ServingUnitModel>().FromSqlRaw(@"SELECT ServingUnitId,ServingUnitName,Description FROM Setup_ServingUnit").ToListAsync();

                return Ok(servingUnit);
            }
        }
}
