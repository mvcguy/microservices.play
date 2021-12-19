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
            var metadata = await categoriesClient.GetProductCategoriesMetaData();
            if (page > metadata.TotalPages)
            {
                page = metadata.TotalPages;
            }
            return View(new ProductCategoriesVm
            {
                PageNumber = page,
                TotalPages = metadata.TotalPages,
                ProductCategories = items
            });
        }

        [HttpGet]
        public async Task<IActionResult> Products(Guid? categoryId, int page = 1)
        {
            var items = await categoriesClient.GetProductsByCategory(categoryId.GetValueOrDefault(), page);
            var metadata = await categoriesClient.GetProductsMetaData();
            if (page > metadata.TotalPages)
            {
                page = metadata.TotalPages;
            }
            return View(new ProductVm
            {
                CategoryId = categoryId.GetValueOrDefault(),
                PageNumber = page,
                TotalPages = metadata.TotalPages,
                Products = items
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

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
