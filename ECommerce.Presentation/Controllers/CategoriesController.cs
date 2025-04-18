﻿using ECommerce.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public CategoriesController(IServiceManager manager)
        {
            _manager = manager;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            return Ok(await _manager
                .CategoryService
                .GetAllAsync(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryAsync([FromRoute] int id)
        {
            return Ok(await _manager
                .CategoryService
                .GetByIdAsync(id,false));
        }

    }
}
