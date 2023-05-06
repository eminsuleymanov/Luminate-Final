using System;
using LuminateFinalProject.Models;

namespace LuminateFinalProject.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        public IDictionary<string, string> Settings { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

