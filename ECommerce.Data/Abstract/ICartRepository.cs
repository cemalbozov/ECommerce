using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
        Cart GetByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(int cartId);
    }
}
