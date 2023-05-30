using Exam.Models;
using Exam.VievModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                UserName = register.Username,
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email


            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            await _signInManager.SignInAsync(user, isPersistent: true);


            return RedirectToAction("Index", "Home", new { area = "" });



        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(login.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(login.UsernameOrEmail);
                if (user == null) { ModelState.AddModelError(string.Empty, "Username , Email or Password is Inccorrect"); }

            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false);
            if (!result.Succeeded)
            {
               
                {
                    ModelState.AddModelError(string.Empty, "Username , Email or Password is Inccorrect");
                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}