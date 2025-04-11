using ECommerce.Business.Abstract;
using ECommerce.Entity;
using ECommerce.Entity.DTO;
using ECommerce.Entity.Exceptions;
using ECommerce.Entity.RequestFeatures;
using ECommerce.Presentation.ActionFilters;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/products")]
    [ApiExplorerSettings(GroupName = "v1")]
    //[ResponseCache(CacheProfileName = "5mins")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge =80)]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public ProductsController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpHead]
        [HttpGet(Name = "GetAllProductsAsync")]
        //[ResponseCache(Duration =60)]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery]ProductParameters productParameters)
        {
            var pagedResult = await _manager
                .ProductService.GetAllAsync(productParameters,false);
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.products);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetAllProductsWithDetailsAsync()
        {
            return Ok(await _manager
                .ProductService
                .GetAllWithDetailsAsync(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductAsync([FromRoute(Name = "id")] int id)
        {
            var productDto = await _manager.ProductService.GetByIdAsync(id,false);
            return Ok(productDto);
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "CreateProductAsync")]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductDtoForInsertion productDto)
        {
            //Validation filter

            /*if (productDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);*/

            var product = await _manager.ProductService.CreateAsync(productDto);
            return StatusCode(201, product);
            //return CreatedAtAction(nameof(GetProduct), new { id = entity.Id }, ProductToDTO(entity));
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute(Name = "id")]int id, 
            [FromBody]ProductDtoForUpdate entityDto)
        {

            await _manager.ProductService.UpdateAsync(id, entityDto, false);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {

            await _manager.ProductService.DeleteAsync(id, false);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateProductAsync([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<ProductDtoForUpdate> entityPatch)
        {
            if(entityPatch is null)
                return BadRequest();

            var result = await _manager.ProductService.GetForPatchAsync(id, true);
            entityPatch.ApplyTo(result.productDtoForUpdate, ModelState);

            TryValidateModel(result.productDtoForUpdate);
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.ProductService.SaveForPatchAsync(result.productDtoForUpdate, result.product);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTİONS");
            return Ok();
        }

    }
}
