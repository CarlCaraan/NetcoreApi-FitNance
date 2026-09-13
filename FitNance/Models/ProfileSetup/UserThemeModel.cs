using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitNance.Models.ProfileSetup
{
    [Table("UserTheme")]
    public class UserThemeModel
    {
        [Key]
        public string UserId { get; set; }

        public string ThemeMode { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public string PrimaryFontColor { get; set; }

        public string SecondaryFontColor { get; set; }

        public string AccentColor { get; set; }
    }
}