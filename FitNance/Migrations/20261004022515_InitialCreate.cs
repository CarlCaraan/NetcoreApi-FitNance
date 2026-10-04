using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitNance.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLevels",
                columns: table => new
                {
                    ActivityLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityLevelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLevels", x => x.ActivityLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Master_Food",
                columns: table => new
                {
                    FoodId = table.Column<string>(type: "nchar(10)", nullable: false),
                    FoodName = table.Column<string>(type: "varchar(150)", nullable: false),
                    Category = table.Column<string>(type: "varchar(100)", nullable: true),
                    ServingSize = table.Column<double>(type: "float", nullable: true),
                    ServingUnit = table.Column<string>(type: "varchar(30)", nullable: true),
                    ServingGrams = table.Column<double>(type: "float", nullable: true),
                    Calories = table.Column<double>(type: "float", nullable: true),
                    Protein = table.Column<double>(type: "float", nullable: true),
                    Carbs = table.Column<double>(type: "float", nullable: true),
                    Fat = table.Column<double>(type: "float", nullable: true),
                    Fiber = table.Column<double>(type: "float", nullable: true),
                    Sodium = table.Column<double>(type: "float", nullable: true),
                    Sugar = table.Column<double>(type: "float", nullable: true),
                    Cholesterol = table.Column<double>(type: "float", nullable: true),
                    IsCanned = table.Column<bool>(type: "bit", nullable: true),
                    IsFastFood = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "varchar(150)", nullable: true),
                    Rowstamp = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_Food", x => x.FoodId);
                });

            migrationBuilder.CreateTable(
                name: "NutritionGoals",
                columns: table => new
                {
                    GoalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GoalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionGoals", x => x.GoalId);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSetupResults",
                columns: table => new
                {
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    BMR = table.Column<double>(type: "float", nullable: false),
                    ActivityFactor = table.Column<double>(type: "float", nullable: false),
                    TDEE = table.Column<double>(type: "float", nullable: false),
                    FitnessGoalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalorieAdjustment = table.Column<double>(type: "float", nullable: false),
                    TargetCalories = table.Column<double>(type: "float", nullable: false),
                    TargetProtein = table.Column<double>(type: "float", nullable: false),
                    TargetFat = table.Column<double>(type: "float", nullable: false),
                    TargetCarbs = table.Column<double>(type: "float", nullable: false),
                    ActivityLevelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoalName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ServingUnits",
                columns: table => new
                {
                    ServingUnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServingUnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServingUnits", x => x.ServingUnitId);
                });

            migrationBuilder.CreateTable(
                name: "Setup_FoodCategory",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setup_FoodCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Rowstamp = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsProfileComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Rowstamp);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Age = table.Column<double>(type: "float", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    CurrentBMI = table.Column<double>(type: "float", nullable: true),
                    ActivityLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FitnessGoalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyIncome = table.Column<double>(type: "float", nullable: true),
                    SavingsGoal = table.Column<double>(type: "float", nullable: true),
                    CurrentSavings = table.Column<double>(type: "float", nullable: true),
                    BMR = table.Column<double>(type: "float", nullable: true),
                    TDEE = table.Column<double>(type: "float", nullable: true),
                    TargetCalories = table.Column<double>(type: "float", nullable: true),
                    TargetProtein = table.Column<double>(type: "float", nullable: true),
                    TargetCarbs = table.Column<double>(type: "float", nullable: true),
                    TargetFat = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserProfileId);
                });

            migrationBuilder.CreateTable(
                name: "UserTheme",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThemeMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryFontColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryFontColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccentColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTheme", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLevels");

            migrationBuilder.DropTable(
                name: "Master_Food");

            migrationBuilder.DropTable(
                name: "NutritionGoals");

            migrationBuilder.DropTable(
                name: "ProfileSetupResults");

            migrationBuilder.DropTable(
                name: "ServingUnits");

            migrationBuilder.DropTable(
                name: "Setup_FoodCategory");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserTheme");
        }
    }
}
