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
        public async Task<IActionResult> Index(int? categoryId,int pageIndex=1)
        {
            IEnumerable<Product> products = _context.Products
                .Where(p => p.IsDeleted == false);
            ViewBag.categoryId = categoryId;
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
            IEnumerable<Product> products =  _context.Products
                .Where(p => p.IsDeleted == false);
            if (categoryId !=null)
            {
                products = products.Where(p => p.CategoryId == (int)categoryId).ToList();
                ViewBag.categoryId = categoryId;

            }
            //ViewBag.totalPages =(int)Math.Ceiling((decimal)products.Count() / 6) ;
            products = products.Skip((pageIndex - 1) * 6).Take(6);
            //if (materialId != null)
            //{
            //    products = products.Where(p => p.MaterialId == materialId).ToList();
            //    ViewBag.materialId = materialId;

            //}
            ViewBag.pageIndex = pageIndex;
            ShopVM shopVM = new ShopVM
            {
                Products = PagenatedList<Product>.Create(products, pageIndex, 6),
                Materials = await _context.Materials.Where(m => m.IsDeleted == false).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync()
            };

            return PartialView("_ShopListPartial",shopVM);
        }
    }
}

