using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LuminateFinalProject.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {

        public async Task<IActionResult> Register()
        {

            return View();
        }
    }
}

