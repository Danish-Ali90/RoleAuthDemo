using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleAuthDemo.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Plot")]
        public int PlotId { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // 🔗 Navigation properties
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [ForeignKey("PlotId")]
        public Plot? Plot { get; set; }
    }
}
