using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
            await _userManager.AddToRoleAsync(appUser, "Member");
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

            Microsoft.AspNetCore.Identity.SignInResult signInResult =await _signInManager
                .PasswordSignInAsync(appUser,loginVM.Password,loginVM.RemindMe,true);

           
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"Your Account has been blocked. It will be active again after {appUser.LockoutEnd} ");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect ");
                return View(loginVM);
            }
            if (await _userManager.IsInRoleAsync(appUser, "SuperAdmin"))
            {
                return RedirectToAction("index", "dashboard", new { area = "manage" });
            }

            return RedirectToAction("profile","Account");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());
            
            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Username = appUser.UserName,
                Email = appUser.Email,
                
                
                
            };
            
            //ProfileVM profileVM = new ProfileVM
            //{
            //    Addresses = appUser.Addresses

            //};
            return View(profileVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile(ProfileVM profileVM)
        {
            if (!ModelState.IsValid)
            {
                
                return View(profileVM);
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (profileVM.Name !=null)
            {
                appUser.Name = profileVM.Name;
            }
            if (profileVM.Surname !=null)
            {
                appUser.Surname = profileVM.Surname;
            }
            
            
            if (appUser.NormalizedEmail != profileVM.Email.Trim().ToUpperInvariant())
            {
                appUser.Email = profileVM.Email; 
            }
            if (appUser.NormalizedUserName != profileVM.Username.Trim().ToUpperInvariant())
            {
                appUser.UserName = profileVM.Username;
            }


            IdentityResult identityResult  = await _userManager.UpdateAsync(appUser);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {

                    ModelState.AddModelError("", identityError.Description);
                }
                return View(profileVM);
            }
            await _signInManager.SignInAsync(appUser, true);
            if (!string.IsNullOrWhiteSpace(profileVM.OldPassword))
            {
                if (!await _userManager.CheckPasswordAsync(appUser,profileVM.OldPassword))
                {
                    ModelState.AddModelError("OldPassword","Old Password is incorrect");
                    return View(profileVM);
                }
                if (profileVM.OldPassword==profileVM.NewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Old Password and New Password cannot be similar");
                    return View(profileVM);

                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                identityResult = await _userManager.ResetPasswordAsync(appUser,token,profileVM.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach (IdentityError identityError in identityResult.Errors)
                    {

                        ModelState.AddModelError("", identityError.Description);
                    }
                    return View(profileVM);
                }
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }
            
            return RedirectToAction("Index","Home");
        }


    }
}

