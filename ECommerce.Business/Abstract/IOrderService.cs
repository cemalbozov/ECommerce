using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}
