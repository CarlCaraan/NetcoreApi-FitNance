using System.ComponentModel.DataAnnotations;

namespace FitNance.Models.Setup
{
    public class ServingUnitModel
    {
        [Key]
        public string ServingUnitId { get; set; } = string.Empty;
        public string ServingUnitName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
