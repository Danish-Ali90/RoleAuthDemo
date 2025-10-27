using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleAuthDemo.Models; // EditUserRolesViewModel + RoleSelection
using RoleAuthDemo.ViewModels; // if you have other viewmodels like UserViewModel here
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleAuthDemo.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // -----------------------
        // GET: /Users
        // -----------------------
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        // -----------------------
        // GET: /Users/Create
        // -----------------------
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View();
        }

        // -----------------------
        // POST: /Users/Create
        // -----------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                CNIC = model.CNIC,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
                TempData["Success"] = "User created successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }


        // ✅ GET: Edit Roles
        public async Task<IActionResult> EditRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = allRoles.Select(role => new RoleSelection
                {
                    RoleName = role!,
                    Selected = userRoles.Contains(role!)
                }).ToList()
            };

            return View(model);
        }

        // ✅ POST: Edit Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound("User not found.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.Roles.Where(r => r.Selected).Select(r => r.RoleName).ToList();

            var addedRoles = selectedRoles.Except(userRoles);
            var removedRoles = userRoles.Except(selectedRoles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            TempData["Success"] = "User roles updated successfully!";
            return RedirectToAction("Index");
        }


        // -----------------------
        // GET: /Users/Delete/{id}
        // -----------------------
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return View(user);
        }

        // -----------------------
        // POST: /Users/Delete
        // -----------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
