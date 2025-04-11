using ECommerce.Business.Abstract;
using ECommerce.Data.Abstract;
using ECommerce.Entity.Models;
using ECommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    public class HomeController:Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult Index()
        {

            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()
            };
            return View(productViewModel);
        }

        public async Task<IActionResult> GetProductsFromRestApi()
        {
            var products = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync("http://localhost:4200/api/products"))
                {
                    string apiResponse= await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(products);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
