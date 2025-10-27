using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleAuthDemo.Models
{
    public class Plot
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string PlotNumber { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Size (per sq. feet) is required")]
        [Display(Name = "Size (Sq. Feet)")]
        [Column("SizePerSqFt")] // 👈 Matches your DB column name exactly
        public double SizePerSqFeet { get; set; }  // ✅ Can store decimals (e.g., 120.50)
    }
}
