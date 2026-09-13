using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.Authentication
{
    public class UserLoginModel
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Rowstamp { get; set; }

        public bool IsProfileComplete { get; set; }
    }
}
