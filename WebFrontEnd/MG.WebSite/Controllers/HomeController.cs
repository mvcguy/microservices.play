using MG.WebSite.Models;
using MG.WebSite.WebClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MG.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductCategoriesClient categoriesClient;

        public HomeController(ILogger<HomeController> logger, 
            ProductCategoriesClient categoriesClient)
        {
            _logger = logger;
            this.categoriesClient = categoriesClient;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var items = await categoriesClient.GetProductCategories(page);
            return View(new ProductCategoriesVm
            {
                PageNumber = page,
                TotalPages = 100,
                ProductCategories = items
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
