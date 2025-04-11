using ECommerce.Business.Abstract;
using ECommerce.Entity.Models;
using ECommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult List(string category,int page=1)
        {
            const int pageSize = 3;
            var productViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            };
            return View(productViewModel);
        }
        public IActionResult Details(string url)
        {
            if (url==null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);

            if (product == null)
            {
                return NotFound();
            }

            ProductDetailModel model = new ProductDetailModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList(),
                ParentCategories = product.ProductCategories.Select(i => i.Category).Select(i => i.ParentCategory).ToList()
            };

            return View(model);
        }
        public IActionResult Search(string q, int page = 1)
        {
            const int pageSize = 3;
            List<Product> products = _productService.GetSearchResult(q.ToLower());
            var productViewModel = new ProductListViewModel()
            {

                PageInfo = new PageInfo()
                {
                    TotalItems = products.Count(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    SearchResult = q
                },
                Products = products
            };
            return View("List",productViewModel);
        }
    }
}
