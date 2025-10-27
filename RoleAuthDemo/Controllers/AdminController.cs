using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleAuthDemo.Models;

namespace RoleAuthDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var totalUsers = _userManager.Users.Count();
            ViewBag.TotalUsers = totalUsers;
            return View();
        }

        // ✅ New Action: User Count Page;;
        public async Task<IActionResult> UserCount()
        {
            var allRoles = _roleManager.Roles.ToList();
            var userCountsByRole = new Dictionary<string, int>();

            foreach (var role in allRoles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                userCountsByRole[role.Name] = usersInRole.Count;
            }

            ViewBag.TotalUsers = _userManager.Users.Count();
            ViewBag.UserCountsByRole = userCountsByRole;

            return View();
        }
    }

}
