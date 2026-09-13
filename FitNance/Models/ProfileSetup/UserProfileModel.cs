using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.ProfileSetup
{
    [Table("UserProfile")]
    public class UserProfileModel
    {
        [Key]
        public string? UserProfileId { get; set; }

        public DateTime? Birthdate { get; set; }

        public double? Age { get; set; }

        public string? Gender { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }

        public double? CurrentBMI { get; set; }

        public string? ActivityLevelId { get; set; }

        public string? FitnessGoalId { get; set; }

        public double? MonthlyIncome { get; set; }

        public double? SavingsGoal { get; set; }

        public double? CurrentSavings { get; set; }

        public double? BMR { get; set; }

        public double? TDEE { get; set; }

        public double? TargetCalories { get; set; }

        public double? TargetProtein { get; set; }

        public double? TargetCarbs { get; set; }

        public double? TargetFat { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }
    }
}
