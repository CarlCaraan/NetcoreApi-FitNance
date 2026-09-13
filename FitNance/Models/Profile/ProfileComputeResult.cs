namespace FitNance.Models.Profile
{
    public class ProfileComputeResult
    {
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMR { get; set; }
        public double ActivityFactor { get; set; }
        public double TDEE { get; set; }
        public string FitnessGoalId { get; set; } = string.Empty;
        public double CalorieAdjustment { get; set; }
        public double TargetCalories { get; set; }
        public double TargetProtein { get; set; }
        public double TargetFat { get; set; }
        public double TargetCarbs { get; set; }
        public string ActivityLevelName { get; set; } = string.Empty;
        public string GoalName { get; set; } = string.Empty;
    }
}
