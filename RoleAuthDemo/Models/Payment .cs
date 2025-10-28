using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleAuthDemo.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }

        [Required]
        [Range(0.01, 1_000_000)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}