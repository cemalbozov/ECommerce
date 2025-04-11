using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewModels
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartItemModel> CartItems { get; set; }

        public double TotalPrice()
        {
            double sum = 0;
            foreach (var item in CartItems)
            {
                if (item.Discount)
                    sum += (double)item.DiscPrice * item.Quantity;
                else
                    sum += (double)item.Price * item.Quantity;
            }
            return sum;
        }
        public int TotalProduct()
        {
            return CartItems.Sum(i => i.Quantity);
        }
    }

    public class CartItemModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public Brand Brand { get; set; }
        public bool Discount { get; set; }
        public double? DiscPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }

    }
}
