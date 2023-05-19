﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LuminateFinalProject.DataAccessLayer;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.BasketViewModels;
using LuminateFinalProject.ViewModels.ProductReviewVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LuminateFinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> AddReview(Review review)
        {
            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .Include(p => p.Material)
                .Include(p => p.Category)
                .Include(r => r.Reviews.Where(r => r.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == review.ProductId);

            ProductReviewVM productReviewVM = new ProductReviewVM { Product = product, Review = review };

            if (!ModelState.IsValid) return RedirectToAction("Details", productReviewVM);

            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (productReviewVM != null && product.Reviews.Count()>0 && product.Reviews.Any(r=>r.UserId ==appUser.Id))
            {
                ModelState.AddModelError("","You have already wrote a review");
                return View("Details", productReviewVM);
            }
            review.UserId = appUser.Id;
            review.CreatedBy = $"{appUser.Name} {appUser.Surname}";
            review.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Details),new { id = product.Id});
        }



        public IActionResult ChangeBasketProductCount(int? id, int count)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_context.Products.Any(p => p.Id == id))
            {
                return NotFound();
            }
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                basketVMs.Find(p => p.Id == id).Count = count;
                basket = JsonConvert.SerializeObject(basketVMs);
                HttpContext.Response.Cookies.Append("basket", basket);
                foreach (BasketVM basketVM in basketVMs)
                {
                    basketVM.Title = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Title;
                    basketVM.Image = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).MainImage;
                    basketVM.Price = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Price;
                }
                return PartialView("_BasketProductTablePartial", basketVMs);
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult RefreshCartProductCount()
        {
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                basket = JsonConvert.SerializeObject(basketVMs);
                HttpContext.Response.Cookies.Append("basket", basket);
                foreach (BasketVM basketVM in basketVMs)
                {
                    basketVM.Title = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Title;
                    basketVM.Image = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).MainImage;
                    basketVM.Price = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Price;
                }
                return PartialView("_BasketPartial", basketVMs);
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult RefreshCartTotal()
        {
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            decimal totalPrice = 0;
            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (BasketVM basketVM in basketVMs)
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id);
                    if (product != null)
                    {
                        basketVM.Title = product.Title;
                        basketVM.Image = product.MainImage;
                        basketVM.Price = product.Price;
                        totalPrice += (basketVM.Count * (decimal)product.Price);
                    }
                }
            }

            return PartialView("_CartTotalPartial", basketVMs);
        }



    }
}

