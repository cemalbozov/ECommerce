using AutoMapper;
using ECommerce.Business.Abstract;
using ECommerce.Data.Abstract;
using ECommerce.Entity.DTO;
using ECommerce.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Concrete
{
    public class ServiceManager : IServiceManager
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IParentCategoryService _parentCategoryService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authenticationService;

        public ServiceManager(IProductService productService,
            ICategoryService categoryService,
            IParentCategoryService parentCategoryService,
            ICartService cartService, IOrderService orderService,
            IAuthenticationService authenticationService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _parentCategoryService = parentCategoryService;
            _cartService = cartService;
            _orderService = orderService;
            _authenticationService = authenticationService;
        }

        public IProductService ProductService => _productService;

        public ICategoryService CategoryService => _categoryService;

        public IOrderService OrderService => _orderService;

        public ICartService CartService => _cartService;

        public IParentCategoryService ParentCategoryService => _parentCategoryService;

        public IAuthenticationService AuthenticationService => _authenticationService;
    }
}
