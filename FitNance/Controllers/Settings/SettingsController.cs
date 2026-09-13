using FitNance.Data;
using FitNance.Models.Authentication;
using FitNance.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Controllers.Settings
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<UserLoginModel> _passwordHasher;

        public SettingsController(
            AppDbContext context,
            PasswordHasher<UserLoginModel> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ChangePasswordResponseModel>> ChangePassword(ChangePasswordRequestModel request)
        {
            // Kunin ang username ng currently logged-in user mula sa JWT.
            var username = User.FindFirstValue(ClaimTypes.Name);

            // Kapag walang username sa JWT, hindi maitutuloy ang request.
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new ChangePasswordResponseModel
                {
                    Success = false,
                    Message = "User is not authenticated."
                });
            }

            // Hanapin ang user sa database gamit ang username mula sa JWT.
            var user = await _context.UserLogin.FirstOrDefaultAsync(x => x.Username == username);

            // Kapag walang corresponding user sa database.
            if (user == null)
            {
                return NotFound(new ChangePasswordResponseModel
                {
                    Success = false,
                    Message = "User account not found."
                });
            }

            // I-check muna kung tama ang current password.
            var passwordResult = _passwordHasher.VerifyHashedPassword(user,user.Password,request.CurrentPassword);

            // Kapag mali ang current password.
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return BadRequest(new ChangePasswordResponseModel
                {
                    Success = false,
                    Message = "Current password is incorrect."
                });
            }

            // Siguraduhing pareho ang NewPassword at ConfirmPassword.
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new ChangePasswordResponseModel
                {
                    Success = false,
                    Message = "New password and confirm password do not match."
                });
            }

            // I-hash ang bagong password bago ito i-save sa database.
            user.Password = _passwordHasher.HashPassword(
                user,
                request.NewPassword);

            // I-save ang bagong password sa database.
            await _context.SaveChangesAsync();

            // Ibalik ang successful response sa Angular.
            return Ok(new ChangePasswordResponseModel
            {
                Success = true,
                Message = "Password changed successfully."
            });
        }
    }
}
