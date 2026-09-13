using System.ComponentModel.DataAnnotations;

namespace FitNance.Models.Setup
{
    public class NutritionGoalsModel
    {
        [Key]
        public string GoalId { get; set; } = string.Empty;
        public string GoalName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
