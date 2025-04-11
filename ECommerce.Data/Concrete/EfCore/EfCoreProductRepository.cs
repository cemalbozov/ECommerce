using ECommerce.Data.Abstract;
using ECommerce.Data.Concrete.EfCore.Extensions;
using ECommerce.Entity;
using ECommerce.Entity.Models;
using ECommerce.Entity.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Concrete.EfCore
{
    public sealed class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(ShopContext context) : base(context)
        {

        }

        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public void Create(Product entity, int[] categoryIds)
        {
            var product = new Product
            {
                Name = entity.Name,
                Price = entity.Price,
                BrandId = entity.BrandId,
                Description = entity.Description,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                ProductAttributes = entity.ProductAttributes,
                ProductCategories = categoryIds.Select(catid => new ProductCategory
                {
                    CategoryId = catid
                }).ToList()
            };

            ShopContext.Products.Add(product);
        }
        public async Task<PagedList<Product>> GetAllAsync(ProductParameters productParameters,
            bool trackChanges)
        {
           var products = await GetAll(trackChanges)
                .FilterProducts(productParameters.MinPrice,productParameters.MaxPrice)
                .Search(productParameters.SearchTerm)
                .Sort(productParameters.OrderBy)
                .ToListAsync();


            return PagedList<Product>
                .ToPagedList(products,
                productParameters.PageNumber,
                productParameters.PageSize);
        }

        public async Task<List<Product>> GetAllAsync(bool trackChanges)
        {
            return await GetAll(trackChanges)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        public List<Brand> GetAllBrands()
        {
            return ShopContext.Brands.ToList();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync(bool trackChanges)
        {
            return await ShopContext
                .Products
                .Include(p => p.Brand)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToArrayAsync();
        }

        public async Task<Product> GetByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public Product GetByIdWithCategories(int id)
        {
            return ShopContext.Products
                            .Where(p => p.Id == id)
                            .Include(p =>p.Brand)
                            .Include(p=>p.ProductAttributes)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .FirstOrDefault();
        }

        public int GetCountByCategory(string category)
        {
            // asQueryable sonradan işlem yapılacağını belirtiyor. son durumda veritabanına sorgu returnde gidiyor.
            var products = ShopContext
                .Products
                .Where(p=>p.IsApproved)
                .AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(p => p.ProductCategories)
                                    .ThenInclude(p => p.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == category));
            }
            return products.Count();
        }

        public List<Product> GetHomePageProducts()
        {
            return ShopContext.Products.Where(p=>p.IsApproved && p.IsHome).ToList();
        }

        public Product GetProductDetails(string url)
        {
            return ShopContext.Products
                            .Where(i => i.Url == url)
                            .Include(i => i.ProductCategories)
                            .ThenInclude(i => i.Category)
                            .ThenInclude(i => i.ParentCategory)
                            .FirstOrDefault();
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            // asQueryable sonradan işlem yapılacağını belirtiyor. son durumda veritabanına sorgu returnde gidiyor.
            var products = ShopContext
            .Products
            .Where(p=>p.IsApproved)
            .AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                products = products.Include(p => p.ProductCategories)
                                    .ThenInclude(p => p.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == name));
            }
            return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
        }

        public List<Product> GetSearchResult(string searchString)
        {
            // asQueryable sonradan işlem yapılacağını belirtiyor. son durumda veritabanına sorgu returnde gidiyor.
            var products = ShopContext
            .Products
            .Where(p => p.IsApproved && (p.Name.ToLower().Contains(searchString) || p.Brand.Name.ToLower().Contains(searchString) || p.ProductCategories.Any(pc=>pc.Category.Name.ToLower().Contains(searchString))))
            .AsQueryable();
                
            return products.ToList();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            var product = ShopContext.Products
                                .Include(p=>p.ProductCategories)
                                .Include(p=>p.ProductAttributes)
                                .FirstOrDefault(p => p.Id == entity.Id);

            if (product != null)
            {
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.BrandId = entity.BrandId;
                product.Description = entity.Description;
                product.Url = entity.Url;
                product.ImageUrl = entity.ImageUrl;
                product.ProductAttributes = entity.ProductAttributes;
                product.IsApproved = entity.IsApproved;
                product.IsHome = entity.IsHome;

                product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                {
                    ProductId=entity.Id,
                    CategoryId= catid
                }).ToList();

            }
        }
    }
}
