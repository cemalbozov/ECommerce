using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Abstract
{
    public interface IParentCategoryRepository: IRepository<ParentCategory>
    {
        Task<ParentCategory> GetByIdAsync(int id, bool trackChanges);
        List<ParentCategory> GetAllWithChildCategories();
    }
}
