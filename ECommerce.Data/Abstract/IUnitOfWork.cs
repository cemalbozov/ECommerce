using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        ICartRepository Carts { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IParentCategoryRepository ParentCategories { get; }
        IProductRepository Products { get; }
        void Save();
        Task SaveAsync();

    }
}
