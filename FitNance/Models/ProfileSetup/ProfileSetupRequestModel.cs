namespace FitNance.Models.ProfileSetup
{
    public class ProfileSetupRequestModel
    {
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public double Height { get; set; }
        public double Weight { get; set; }
        public string ActivityLevelId { get; set; } = string.Empty;
        public string GoalId { get; set; } = string.Empty;
    }
}
