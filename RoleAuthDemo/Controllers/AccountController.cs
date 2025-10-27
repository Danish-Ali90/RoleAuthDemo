using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleAuthDemo.Models;
using RoleAuthDemo.ViewModels;

namespace RoleAuthDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // ✅ GET: /Account/Login;;
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]   
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }


        // ✅ GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
