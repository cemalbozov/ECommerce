using ECommerce.Entity.DTO;
using ECommerce.Entity.Models;
using ECommerce.Entity.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Abstract
{
    public interface IProductService: IValidator<Product>
    {
        Task<ProductDto> GetByIdAsync(int id, bool trackChanges);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        Task<(IEnumerable<ExpandoObject>products,MetaData metaData)> GetAllAsync(ProductParameters productParameters,bool trackChanges);
        Task<(ProductDtoForUpdate productDtoForUpdate, Product product)> GetForPatchAsync(int id, bool trackChanges);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        bool Create(Product entity);
        Task<ProductDto> CreateAsync(ProductDtoForInsertion product);
        void Update(Product entity);
        Task UpdateAsync(int id,ProductDtoForUpdate productDto, bool trackChanges);
        void Delete(Product entity);
        Task DeleteAsync(int id, bool trackChanges);
        int GetCountByCategory(string category);
        bool Create(Product entity, int[] categoryIds);
        bool Update(Product entity, int[] categoryIds);
        Task SaveForPatchAsync(ProductDtoForUpdate productDtoForUpdate,Product product);
        List<Brand> GetAllBrands();
        Task<List<Product>> GetAllAsync(bool trackChanges);
        Task<IEnumerable<ProductDto>> GetAllWithDetailsAsync(bool trackChanges);
    }
}
