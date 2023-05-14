using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuminateFinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.Username
            };
            //if (!await  _userManager.Users.AnyAsync(e=>e.NormalizedEmail == registerVM.Email.ToUpperInvariant().Trim()))
            //{
            //    ModelState.AddModelError("Email", $"{registerVM.Email} is already taken");
            //    return View(registerVM);
            //}

            //if (!await _userManager.Users.AnyAsync(e => e.NormalizedUserName == registerVM.Username.ToUpperInvariant().Trim()))
            //{
            //    ModelState.AddModelError("Username", $"{registerVM.Username} is already taken");
            //    return View(registerVM);
            //}
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Admin");
            return RedirectToAction(nameof(Login));
        }

        //[HttpGet]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Content("Successfully");

        //}

        //[HttpGet]
        //public async Task<IActionResult> CreateUser()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        Surname = "Admin",
        //        Email = "superadmin@gmail.com",
        //        UserName = "SuperAdmin"
        //    };

        //    await _userManager.CreateAsync(appUser, "SuperAdmin17");
        //    await _userManager.AddToRoleAsync(appUser,"SuperAdmin");

        //    return Content("Role added successfully");

        //}

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser==null)
            {
                ModelState.AddModelError("","Email or Password is incorrect ");
                return View(loginVM);

            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult =await _signInManager.PasswordSignInAsync(appUser,loginVM.Password,true,true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect ");
                return View(loginVM);
            }


            return RedirectToAction("Index","Home");
        }



    }
}

