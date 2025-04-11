using ECommerce.Data.Abstract;
using ECommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(ShopContext context) : base(context)
        {

        }
        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public void DeleteFromCategory(int productId, int categoryId)
        {
            ProductCategory pc = ShopContext.ProductCategory.Where(pc => pc.CategoryId == categoryId && pc.ProductId == productId).FirstOrDefault();

            ShopContext.ProductCategory.Remove(pc);
        }
        public async Task<Category> GetByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
        public Category GetByIdWithParentCAndProducts(int id)
        {
            return ShopContext.Categories
                            .Where(c => c.Id == id)
                            .Include(c => c.ParentCategory)
                            .Include(c => c.ProductCategories)
                            .ThenInclude(pc => pc.Product)
                            .FirstOrDefault();
        }

        public List<Category> GetPopularCategories()
        {
            throw new NotImplementedException();
        }
    }
}
