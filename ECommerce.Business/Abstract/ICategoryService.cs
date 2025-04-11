using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        Task<Category> GetByIdAsync(int id, bool trackChanges);
        Category GetByIdWithParentCAndProducts(int id);
        List<Category> GetAll();
        Task<IEnumerable<Category>> GetAllAsync(bool trackChanges);
        void Create(Category entity);
        Task<Category> CreateAsync(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int productId,int categoryId);
    }
}
