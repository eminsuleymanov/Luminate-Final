using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels;
using LuminateFinalProject.ViewModels.ShopViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuminateFinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pageIndex=1)
        {
            IQueryable<Product> products = _context.Products
                .Where(p => p.IsDeleted == false);

            ShopVM shopVM = new ShopVM
            {
                Products = PagenatedList<Product>.Create(products, pageIndex, 6),
                Materials = await _context.Materials.Where(m=>m.IsDeleted==false).ToListAsync(),
                Categories = await _context.Categories.Where(c=>c.IsDeleted==false).ToListAsync()
            };
            return View(shopVM);
        }

        public async Task<IActionResult> ShopFilters(int? categoryId,int? materialId,int pageIndex = 1)
        {

            return PartialView("_ShopListPartial");
        }
    }
}

