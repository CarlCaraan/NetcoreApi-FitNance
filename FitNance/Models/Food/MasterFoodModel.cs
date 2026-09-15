using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.Food
{
    [Table("Master_Food")]
    public class MasterFoodModel
    {
        [Key]
        [Column(TypeName = "nchar(10)")]
        public string? FoodId { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string FoodName { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string? Category { get; set; }

        public double? ServingSize { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? ServingUnit { get; set; }

        public double? ServingGrams { get; set; }

        public double? Calories { get; set; }

        public double? Protein { get; set; }

        public double? Carbs { get; set; }

        public double? Fat { get; set; }

        public double? Fiber { get; set; }

        public double? Sodium { get; set; }

        public double? Sugar { get; set; }

        public double? Cholesterol { get; set; }

        public bool? IsCanned { get; set; }

        public bool? IsFastFood { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? UserId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Rowstamp { get; set; }
    }
}
