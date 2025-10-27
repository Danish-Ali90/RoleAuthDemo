using System;
using Microsoft.AspNetCore.Identity;

namespace RoleAuthDemo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CNIC { get; set; }





        // 🔹 Add CreatedAt and UpdatedAt fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
