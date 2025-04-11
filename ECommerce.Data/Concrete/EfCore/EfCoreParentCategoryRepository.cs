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
    public class EfCoreParentCategoryRepository : EfCoreGenericRepository<ParentCategory>, IParentCategoryRepository
    {
        public EfCoreParentCategoryRepository(ShopContext context) : base(context)
        {

        }
        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public async Task<ParentCategory> GetByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public List<ParentCategory> GetAllWithChildCategories()
        {
                return ShopContext.ParentCategories.Include(p => p.Categories).ToList();
        }
    }
}
