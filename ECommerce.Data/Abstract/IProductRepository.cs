using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Entity.Models;
using ECommerce.Entity.RequestFeatures;

namespace ECommerce.Data.Abstract
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<PagedList<Product>> GetAllAsync(ProductParameters productParameters, bool trackChanges);
        Task<IEnumerable<Product>> GetAllWithDetailsAsync(bool trackChanges);
        Task<List<Product>> GetAllAsync(bool trackChanges);
        Task<Product> GetByIdAsync(int id, bool trackChanges);
        Product GetProductDetails(string url);
        Product GetByIdWithCategories(int id);
        List<Product> GetProductsByCategory(string name,int page,int pageSize);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetHomePageProducts();
        int GetCountByCategory(string category);
        void Create(Product entity, int[] categoryIds);
        void Update(Product entity, int[] categoryIds);
        List<Brand> GetAllBrands();
    }
}
