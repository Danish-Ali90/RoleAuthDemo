using System;
using System.ComponentModel.DataAnnotations;

namespace RoleAuthDemo.ViewModels
{
    public class CreatePaymentViewModel
    {
        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

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
    }
}