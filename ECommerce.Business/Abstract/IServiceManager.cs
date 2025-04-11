using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Abstract
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        ICategoryService CategoryService { get; }
        IOrderService OrderService { get; }
        ICartService CartService { get; }
        IParentCategoryService ParentCategoryService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
