using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.MealPlan;

[Table("MealPlan_H", Schema = "dbo")]
public class MealPlanHeader
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "numeric(18,0)")]
    public decimal RowStamp { get; set; }
    [Column(TypeName = "varchar(30)")]
    public string MealPlanId { get; set; } = string.Empty;
    [Column(TypeName = "varchar(30)")]
    public string UserProfileId { get; set; } = string.Empty;
    [Column(TypeName = "varchar(150)")]
    public string PlanName { get; set; } = string.Empty;
    public double? TotalCalories { get; set; }
    public double? TotalProtein { get; set; }
    public double? TotalCarbs { get; set; }
    public double? TotalFat { get; set; }
    public double? TotalFiber { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string? CreatedBy { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string? ModifiedBy { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public List<MealPlanDetail> Details { get; set; } = [];
}

[Table("MealPlan_D", Schema = "dbo")]
public class MealPlanDetail
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "numeric(18,0)")]
    public decimal RowStamp { get; set; }
    [Column(TypeName = "varchar(30)")]
    public string MealPlanDetailId { get; set; } = string.Empty;
    [Column(TypeName = "varchar(30)")]
    public string MealPlanId { get; set; } = string.Empty;
    [Column(TypeName = "varchar(30)")]
    public string FoodId { get; set; } = string.Empty;
    [Column(TypeName = "varchar(50)")]
    public string MealType { get; set; } = string.Empty;
    public double? Quantity { get; set; }
    public double? QuantityGrams { get; set; }
    public double? Calories { get; set; }
    public double? Protein { get; set; }
    public double? Carbs { get; set; }
    public double? Fat { get; set; }
    public double? Fiber { get; set; }
    public double? Sugar { get; set; }
    public double? Sodium { get; set; }
    public DateOnly? MealDate { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string? CreatedBy { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string? ModifiedBy { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }
}

public record MealPlanRequest(
    [Required, StringLength(150)] string PlanName,
    DateOnly? StartDate,
    DateOnly? EndDate,
    [Required] List<MealPlanDetailRequest> Details);

public record MealPlanDetailRequest(
    [StringLength(30)] string? MealPlanDetailId,
    [Required, StringLength(30)] string FoodId,
    [Required, StringLength(50)] string MealType,
    DateOnly? MealDate,
    [Range(0.000001, 1000000)] double? Quantity);
