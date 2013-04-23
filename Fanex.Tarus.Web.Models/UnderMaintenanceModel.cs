namespace Fanex.Tarus.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UnderMaintenanceModel
    {
        [Required]
        [Display(Name = "From")]
        public string From { get; set; }

        [Required]
        [Display(Name = "To")]
        public string To { get; set; }
    }
}