namespace FitNance.Models.Profile
{
    public class UpdateProfileRequest
    {
        public DateTime Birthdate { get; set; }
        public float Age { get; set; }
        public string Gender { get; set; } = string.Empty;

        public float Height { get; set; }
        public float Weight { get; set; }
        public float CurrentBMI { get; set; }

        public string ActivityLevelId { get; set; } = string.Empty;
        public string FitnessGoalId { get; set; } = string.Empty;

        public float MonthlyIncome { get; set; }
        public float SavingsGoal { get; set; }
        public float CurrentSavings { get; set; }

        public float TargetCalories { get; set; }
        public float TargetProtein { get; set; }
        public float TargetCarbs { get; set; }
        public float TargetFat { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
