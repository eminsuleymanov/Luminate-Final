using System;
using LuminateFinalProject.Models;
using LuminateFinalProject.ViewModels.BasketViewModels;

namespace LuminateFinalProject.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<BasketVM> BasketVMs { get; set; }

    }
}

