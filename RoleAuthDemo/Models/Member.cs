using System.ComponentModel.DataAnnotations;

namespace RoleAuthDemo.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(15)]
        public string CNIC { get; set; }

        [Required, StringLength(15)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, StringLength(50)]
        public string City { get; set; }

        // ✅ New property for profile image
        [Display(Name = "Profile Picture")]
        public string? ProfileImagePath { get; set; }
    }
}
