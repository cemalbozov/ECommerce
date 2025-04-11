using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Abstract
{
    public interface IParentCategoryService: IValidator<ParentCategory>
    {
        Task<ParentCategory> GetByIdAsync(int id,bool trackChanges);
        List<ParentCategory> GetAll(bool trackChanges);
        List<ParentCategory> GetAllWithCategories();
        void Create(ParentCategory entity);
        void Update(ParentCategory entity);
        void Delete(ParentCategory entity);
    }
}
