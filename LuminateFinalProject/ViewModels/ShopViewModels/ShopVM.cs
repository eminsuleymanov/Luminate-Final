using System;
using LuminateFinalProject.Models;

namespace LuminateFinalProject.ViewModels.ShopViewModels
{
    public class ShopVM
    {
        public PagenatedList<Product> Products { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
        
        public IEnumerable<Material> Materials { get; set; }



    }
}

