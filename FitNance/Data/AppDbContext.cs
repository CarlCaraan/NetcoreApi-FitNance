using FitNance.Models.Authentication;
using FitNance.Models.ProfileSetup;
using FitNance.Models.Setup;
using FitNance.Models.Food;
using FitNance.Models.MealPlan;
using Microsoft.EntityFrameworkCore;

namespace FitNance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //SETUPs
        public DbSet<ActivityLevelModel> ActivityLevels { get; set; }
        public DbSet<NutritionGoalsModel> NutritionGoals { get; set; }
        public DbSet<ServingUnitModel> ServingUnits { get; set; }
        public DbSet<FoodCategoryModel> FoodCategories { get; set; }




        public DbSet<UserLoginModel> UserLogin { get; set; }
        public DbSet<ProfileSetupResultModel> ProfileSetupResults { get; set; }
        public DbSet<UserProfileModel> UserProfiles { get; set; }
        public DbSet<UserThemeModel> UserThemes { get; set; }
        public DbSet<MasterFoodModel> MasterFoods { get; set; }

        public DbSet<MealPlanHeader> MealPlans => Set<MealPlanHeader>();
        public DbSet<MealPlanDetail> MealPlanDetails => Set<MealPlanDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealPlanHeader>()
                .HasMany(plan => plan.Details)
                .WithOne()
                .HasPrincipalKey(plan => plan.MealPlanId)
                .HasForeignKey(detail => detail.MealPlanId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ProfileSetupResultModel>().HasNoKey(); // Para lang malaman ni EF Core na yung result ay hindi actual table at wag na hanapan ng Primary Key
        }

    }
}
