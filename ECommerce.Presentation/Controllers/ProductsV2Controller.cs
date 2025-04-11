using ECommerce.Business.Abstract;
using ECommerce.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiVersion("2.0",Deprecated = true)]
    [ApiController]
    [Route("api/products")]
    [ApiExplorerSettings(GroupName ="v2")]
    public class ProductsV2Controller: ControllerBase
    {
        private readonly IServiceManager _manager;

        public ProductsV2Controller(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _manager.ProductService.GetAllAsync(false);
            var productsV2 = products.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
            });
            return Ok(productsV2);
        }
    }
}
