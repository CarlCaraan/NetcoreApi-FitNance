using FitNance.Models.Authentication;
using FitNance.Models.ProfileSetup;
using FitNance.Models.Setup;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserLoginModel> UserLogin { get; set; }
        public DbSet<ActivityLevelModel> ActivityLevels { get; set; }
        public DbSet<NutritionGoalsModel> NutritionGoals { get; set; }
        public DbSet<ProfileSetupResultModel> ProfileSetupResults { get; set; }
        public DbSet<UserProfileModel> UserProfiles { get; set; }
        public DbSet<UserThemeModel> UserThemes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfileSetupResultModel>().HasNoKey(); // Para lang malaman ni EF Core na yung result ay hindi actual table at wag na hanapan ng Primary Key
        }

    }
}
