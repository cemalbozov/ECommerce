using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Abstract
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<Category> GetByIdAsync(int id, bool trackChanges);
        List<Category> GetPopularCategories();
        Category GetByIdWithParentCAndProducts(int id);
        void DeleteFromCategory(int productId, int categoryId);
    }
}
