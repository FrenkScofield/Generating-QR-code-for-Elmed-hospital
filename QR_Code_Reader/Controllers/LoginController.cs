using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Code_Reader.Models.DAL;
using QR_Code_Reader.Models.VM;

namespace QR_Code_Reader.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(MyContext context,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            IdentityUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email or Password is invalid");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult =
                  await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Email", "Email or password is invalid");
                return View(loginViewModel);
            }

            //   var rol = await _roleManager.Roles.Where(r => r.N  ame ==  "Admin").FirstOrDefault();

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                return NotFound();
            }

            return RedirectToAction("Index", "QRCode");
        }
    }
}
