namespace FitNance.Models.ProfileSetup
{
    public class CompleteProfileRequestModel
    {
        public DateTime Birthdate { get; set; }
        public float Age { get; set; }
        public string Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float CurrentBMI { get; set; }

        public string ActivityLevelId { get; set; }
        public string FitnessGoalId { get; set; }

        public float MonthlyIncome { get; set; }
        public float SavingsGoal { get; set; }
        public float CurrentSavings { get; set; }

        public float BMR { get; set; }
        public float TDEE { get; set; }
        public float TargetCalories { get; set; }
        public float TargetProtein { get; set; }
        public float TargetCarbs { get; set; }
        public float TargetFat { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        // USER THEME COLUMNS
        public string ThemeMode { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string PrimaryFontColor { get; set; }
        public string SecondaryFontColor { get; set; }
        public string AccentColor { get; set; }

    }
}
