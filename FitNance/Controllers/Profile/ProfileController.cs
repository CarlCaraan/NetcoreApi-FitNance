using FitNance.Data;
using FitNance.Models.Profile;
using FitNance.Models.ProfileSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Controllers.Profile
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userProfileId}")]
        public async Task<IActionResult> GetProfile(string userProfileId)
        {
            var profile = await _context.UserProfiles
                .Where(x => x.UserProfileId == userProfileId)
                .Select(x => new UserProfileModel
                {
                    UserProfileId = x.UserProfileId,
                    Birthdate = x.Birthdate,
                    Age = Math.Round(x.Age ?? 0, 2),
                    Gender = x.Gender,
                    Height = Math.Round(x.Height ?? 0, 2),
                    Weight = Math.Round(x.Weight ?? 0, 2),
                    CurrentBMI = Math.Round(x.CurrentBMI ?? 0, 2),
                    ActivityLevelId = x.ActivityLevelId,
                    FitnessGoalId = x.FitnessGoalId,
                    MonthlyIncome = Math.Round(x.MonthlyIncome ?? 0, 2),
                    SavingsGoal = Math.Round(x.SavingsGoal ?? 0, 2),
                    CurrentSavings = Math.Round(x.CurrentSavings ?? 0, 2),
                    BMR = Math.Round(x.BMR ?? 0, 2),
                    TDEE = Math.Round(x.TDEE ?? 0, 2),
                    TargetCalories = Math.Round(x.TargetCalories ?? 0, 2),
                    TargetProtein = Math.Round(x.TargetProtein ?? 0, 2),
                    TargetCarbs = Math.Round(x.TargetCarbs ?? 0, 2),
                    TargetFat = Math.Round(x.TargetFat ?? 0, 2),
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName
                })
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Profile not found.",
                    userProfileId
                });
            }

            return Ok(profile);
        }


        // ==========================================
        // COMPUTE PROFILE TARGETS
        // ==========================================
        [HttpPost("computeTargets")]
        public async Task<IActionResult> ComputeTargets(
            [FromBody] ProfileComputeRequest request)
        {
            try
            {
                var result = await _context.Database
                    .SqlQueryRaw<ProfileComputeResult>(
                        """
                        EXEC PROC_ProfileSetupResult
                            @Birthdate,
                            @Gender,
                            @Height,
                            @Weight,
                            @ActivityLevelId,
                            @GoalId
                        """,
                        new SqlParameter("@Birthdate", request.BirthDate),
                        new SqlParameter("@Gender", request.Gender),
                        new SqlParameter("@Height", request.Height),
                        new SqlParameter("@Weight", request.Weight),
                        new SqlParameter("@ActivityLevelId", request.ActivityLevelId),
                        new SqlParameter("@GoalId", request.GoalId)
                    )
                    .ToListAsync();

                if (result.Count == 0)
                {
                    return NotFound(new
                    {
                        message = "Unable to compute profile targets."
                    });
                }

                return Ok(result.First());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while computing profile targets.",
                    error = ex.Message
                });
            }
        }






    }
}
