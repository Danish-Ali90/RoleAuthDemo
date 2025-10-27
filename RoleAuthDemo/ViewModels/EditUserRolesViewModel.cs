using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoleAuthDemo.Models
{
    public class EditUserRolesViewModel
    {
        [Required]
        public string? UserId { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        public List<RoleSelection> Roles { get; set; } = new();
    }

    public class RoleSelection
    {
        public string RoleName { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }
}
