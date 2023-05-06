﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuminateFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _context.Products.Where(s => s.IsDeleted == false).ToListAsync();
            products = products.Take(3);


            HomeVM vm = new HomeVM
            {
                Products=products,
                Settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value)

            };

            return View(vm);
        }
    }
}
