using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using ECommerce.Entity.Models;

namespace ECommerce.Data.Concrete.EfCore.Extensions
{
    public static class EfCoreProductRepositoryExtensions
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products,
            uint minPrice, uint maxPrice)
        {
            return products.Where(p =>
            p.Price >= minPrice &&
            p.Price <= maxPrice);
        }

        public static IQueryable<Product> Search(this IQueryable<Product> products,
            string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
                return products;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return products
                .Where(p => p.IsApproved && 
                (p.Name.ToLower().Contains(lowerCaseTerm) || p.Brand.Name.ToLower().Contains(lowerCaseTerm) || p.ProductCategories.Any(pc => pc.Category.Name.ToLower().Contains(lowerCaseTerm))));
        }
        public static IQueryable<Product> Sort(this IQueryable<Product> products,
            string orderByQueryString)
        {
            if(string.IsNullOrWhiteSpace(orderByQueryString))
                return products.OrderBy(b => b.DateAdded);

            var orderQuery = OrderQueryBuilder
                .CreateOrderQuery<Product>(orderByQueryString);

            if(orderQuery is null)
                return products.OrderBy(p => p.DateAdded);

            return products.OrderBy(orderQuery);

        }
    }
}
