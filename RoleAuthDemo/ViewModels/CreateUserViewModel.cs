using System.ComponentModel.DataAnnotations;

namespace RoleAuthDemo.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "CNIC is required")]
        [StringLength(15, MinimumLength = 13, ErrorMessage = "CNIC must be between 13 and 15 characters")]
        public string? CNIC { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role selection is required")]
        public string? Role { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be 10–15 digits")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
