using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuminateFinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .Include(p => p.Material)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
            if (product == null) return NotFound();

            return View(product);

        }
    }
}

