using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.ProductReviewVMs;
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
                .Include(r=>r.Reviews.Where(r=>r.IsDeleted==false))
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
            if (product == null) return NotFound();
            ProductReviewVM productReviewVM = new ProductReviewVM
            {
                Product = product,
                Review = new Review { ProductId = id}
            };
            return View(productReviewVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(Review review)
        {
            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .Include(p => p.Material)
                .Include(p => p.Category)
                .Include(r => r.Reviews.Where(r => r.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == review.ProductId);
            if (!ModelState.IsValid) return RedirectToAction("Details",new ProductReviewVM {Product= product,Review=review });
            return RedirectToAction();
        }

    }
}

