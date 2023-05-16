using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.Areas.Manage.ViewModels.UserViewModels;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuminateFinalProject.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public UserController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    //Not Working
        //    //List<UserVM> users = await _userManager.Users
        //    //    .Where(u=>u.UserName != User.Identity.Name)
        //    //    .Select(x=> new UserVM {
        //    //        Id = x.Id,
        //    //        Email = x.Email,
        //    //        Name = x.Name,
        //    //        Surname = x.Surname,
        //    //        Username = x.UserName
        //    //    })
        //    //    .ToListAsync();

        //    //foreach (var item in users)
        //    //{
        //    //    string roleId = _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == item.Id);
        //    //}
        //    //return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> ChangeRole(string? id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return BadRequest();
        //    AppUser appUser = await _userManager.FindByIdAsync(id);
        //    if (appUser == null) return NotFound();
        //    UserChangeRole userChangeRole = new UserChangeRole
        //    {
        //        UserId = appUser.Id,
        //        RoleId = await _userManager.GetRolesAsync()
        //    };


        //}
    }
}

