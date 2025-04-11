using ECommerce.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext _context;

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParentCategoryRepository _parentCategoryRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public UnitOfWork(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IParentCategoryRepository parentCategoryRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            ShopContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _parentCategoryRepository = parentCategoryRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _context = context;
        }

        public ICartRepository Carts => _cartRepository;

        public ICategoryRepository Categories => _categoryRepository;

        public IOrderRepository Orders => _orderRepository;

        public IParentCategoryRepository ParentCategories => _parentCategoryRepository;

        public IProductRepository Products => _productRepository;

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
