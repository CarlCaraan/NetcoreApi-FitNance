using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.Setup
{
    [Table("Setup_FoodCategory")]
    public class FoodCategoryModel
    {
        [Key]
        public string CategoryId { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
