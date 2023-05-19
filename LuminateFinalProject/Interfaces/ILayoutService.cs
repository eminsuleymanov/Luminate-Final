using System;
using LuminateFinalProject.ViewModels.BasketViewModels;

namespace LuminateFinalProject.Interfaces
{
    public interface ILayoutService
    {
        Task<IEnumerable<BasketVM>> GetBaskets();
    }
}

