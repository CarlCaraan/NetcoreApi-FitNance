using System.Security.Claims;
using FitNance.Data;
using FitNance.Models.ProfileSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Controllers.ProfileSetup
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileSetupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileSetupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("result")]
        public async Task<IActionResult> Result(ProfileSetupRequestModel request)
        {
            var result = await _context.ProfileSetupResults
          .FromSqlRaw("EXEC PROC_ProfileSetupResult @BirthDate, @Gender, @Height, @Weight, @ActivityLevelId, @GoalId",
              new SqlParameter("@BirthDate", request.BirthDate),
              new SqlParameter("@Gender", request.Gender),
              new SqlParameter("@Height", request.Height),
              new SqlParameter("@Weight", request.Weight),
              new SqlParameter("@ActivityLevelId", request.ActivityLevelId),
              new SqlParameter("@GoalId", request.GoalId))
          .AsNoTracking() //Kunin mo lang ang data. Huwag mo itong i-track for changes.
          .ToListAsync();

            var profileResult = result.FirstOrDefault();

            if (profileResult == null)
            {
                return NotFound(new
                {
                    message = "Unable to compute profile setup."
                });
            }

            return Ok(profileResult);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteProfile(
                CompleteProfileRequestModel request)
        {
            // ==========================================
            // GET USERNAME FROM JWT
            // ==========================================

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify the logged-in user."
                });
            }


            // ==========================================
            // FIND USER LOGIN
            // ==========================================

            var user = await _context.UserLogin
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User account not found."
                });
            }


            // ==========================================
            // TRANSACTION
            // ==========================================

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ==========================================
                // CREATE USER PROFILE
                // ==========================================

                var profile = new UserProfileModel
                {
                    UserProfileId = user.Username,

                    Birthdate = request.Birthdate,
                    Age = request.Age,
                    Gender = request.Gender,
                    Height = request.Height,
                    Weight = request.Weight,
                    CurrentBMI = request.CurrentBMI,

                    ActivityLevelId = request.ActivityLevelId,
                    FitnessGoalId = request.FitnessGoalId,

                    MonthlyIncome = request.MonthlyIncome,
                    SavingsGoal = request.SavingsGoal,
                    CurrentSavings = request.CurrentSavings,

                    BMR = request.BMR,
                    TDEE = request.TDEE,
                    TargetCalories = request.TargetCalories,
                    TargetProtein = request.TargetProtein,
                    TargetCarbs = request.TargetCarbs,
                    TargetFat = request.TargetFat,

                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,

                    CreatedBy = username,
                    CreatedDate = DateTime.Now
                };

                _context.UserProfiles.Add(profile);


                // ==========================================
                // UPDATE USER LOGIN
                // ==========================================

                user.IsProfileComplete = true;


                // ==========================================
                // ADD USER THEME
                // ==========================================
                var theme = new UserThemeModel
                {
                    UserId = username,
                    ThemeMode = request.ThemeMode,
                    PrimaryColor = request.PrimaryColor,
                    SecondaryColor = request.SecondaryColor,
                    PrimaryFontColor = request.PrimaryFontColor,
                    SecondaryFontColor = request.SecondaryFontColor,
                    AccentColor = request.AccentColor
                };

                _context.UserThemes.Add(theme);


                // ==========================================
                // SAVE
                // ==========================================

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                // ==========================================
                // RESPONSE
                // ==========================================

                return Ok(new
                {
                    message = "Profile completed successfully.",
                    username = username,
                    isProfileComplete = true
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message = "Unable to complete profile.",
                    error = ex.Message
                });
            }
        }


    }
}
