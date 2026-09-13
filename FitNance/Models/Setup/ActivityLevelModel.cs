using System.ComponentModel.DataAnnotations;
namespace FitNance.Models.Setup
{
    public class ActivityLevelModel
    {
        [Key]
        public string ActivityLevelId { get; set; } = string.Empty;
        public string ActivityLevelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
