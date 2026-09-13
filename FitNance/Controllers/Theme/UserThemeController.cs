using FitNance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitNance.Models.ProfileSetup;

namespace FitNance.Controllers.Theme
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserThemeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserThemeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserTheme(string username)
        {
            var theme = await _context.UserThemes
                .FirstOrDefaultAsync(x => x.UserId == username);

            if (theme == null)
            {
                return NotFound();
            }

            return Ok(theme);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUserTheme(
            string username,
            [FromBody] UserThemeModel request)
        {
            var theme = await _context.UserThemes
                .FirstOrDefaultAsync(x => x.UserId == username);

            if (theme == null)
            {
                return NotFound();
            }

            theme.ThemeMode = request.ThemeMode;
            theme.PrimaryColor = request.PrimaryColor;
            theme.SecondaryColor = request.SecondaryColor;
            theme.PrimaryFontColor = request.PrimaryFontColor;
            theme.SecondaryFontColor = request.SecondaryFontColor;
            theme.AccentColor = request.AccentColor;

            await _context.SaveChangesAsync();

            return Ok(theme);
        }
    }
}
