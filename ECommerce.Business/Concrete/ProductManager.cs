using AutoMapper;
using ECommerce.Business.Abstract;
using ECommerce.Data.Abstract;
using ECommerce.Data.Concrete.EfCore;
using ECommerce.Entity.DTO;
using ECommerce.Entity.Exceptions;
using ECommerce.Entity.Models;
using ECommerce.Entity.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductDto> _shaper;
        public ProductManager(IUnitOfWork unitOfWork, ILoggerService logger,
            IMapper mapper, IDataShaper<ProductDto> shaper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _shaper = shaper;
        }

        public bool Create(Product entity)
        {
            // iş kuralları

            if (Validation(entity))
            {
                _unitOfWork.Products.Create(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
            
        }

        public bool Create(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                _unitOfWork.Products.Create(entity, categoryIds);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public async Task<ProductDto> CreateAsync(ProductDtoForInsertion productDto)
        {
            var categoryIds = productDto.CategoryIds;

            var categories = await _unitOfWork
                .Categories
                .GetAll(false)
                .ToListAsync();

            foreach (var categoryId in categoryIds)
            {
                if (!categories.Any(c => c.Id == categoryId))
                {
                    // Eğer kategori veritabanında bulunamazsa NotFoundException fırlat
                    throw new CategoryNotFoundException(categoryId);
                }
            }

            var entity = _mapper.Map<Product>(productDto);

            entity.ProductCategories = categoryIds.Select(cid => new ProductCategory
            {
                CategoryId = cid,
            }).ToList();

            _unitOfWork.Products.Create(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductDto>(entity);
        }

        public void Delete(Product entity)
        {
            //iş kuralları
            _unitOfWork.Products.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<(IEnumerable<ExpandoObject> products, MetaData metaData)> GetAllAsync(ProductParameters productParameters,
            bool trackChanges)
        {
            if (!productParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var productsWithMetaData = await _unitOfWork.
                Products
                .GetAllAsync(productParameters,trackChanges);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsWithMetaData);

            var shapedData = _shaper.ShapeData(productsDto, productParameters.Fields);
            return(products : shapedData, metaData : productsWithMetaData.MetaData);
        }

        public List<Brand> GetAllBrands()
        {
            return _unitOfWork.Products.GetAllBrands();
        }

        public async Task<ProductDto> GetByIdAsync(int id,bool trackChanges)
        {
            var product = await GetByIdAndCheckExists(id, trackChanges);
            return _mapper.Map<ProductDto>(product);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _unitOfWork.Products.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _unitOfWork.Products.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _unitOfWork.Products.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _unitOfWork.Products.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _unitOfWork.Products.GetProductsByCategory(name,page,pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _unitOfWork.Products.GetSearchResult(searchString);
        }

        public async Task<(ProductDtoForUpdate productDtoForUpdate, Product product)> GetForPatchAsync(int id,bool trackChanges)
        {
            var product = await GetByIdAndCheckExists(id, trackChanges);
            var productDtoForUpdate = _mapper.Map<ProductDtoForUpdate>(product);
            return(productDtoForUpdate, product);
        }

        public void Update(Product entity)
        {
            _unitOfWork.Products.Update(entity);
            _unitOfWork.Save();
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz";
                    return false;
                }
                _unitOfWork.Products.Update(entity, categoryIds);
                _unitOfWork.Save();
                return true;
            }
            return false;
            
        }
        public async Task UpdateAsync(int id,
            ProductDtoForUpdate productDto,
            bool trackChanges)
        {
            var product = await GetByIdAndCheckExists(id, trackChanges);
            product = _mapper.Map<Product>(productDto);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id, bool trackChanges)
        {
            var product = await GetByIdAndCheckExists(id, trackChanges);
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task SaveForPatchAsync(ProductDtoForUpdate productDtoForUpdate, Product product)
        {
            _mapper.Map(productDtoForUpdate, product);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Product> GetByIdAndCheckExists(int id,bool trackChanges)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, trackChanges);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            return product;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Ürün ismi girmelisiniz. \n";
                isValid = false;
            }
            if (entity.Price<0)
            {
                ErrorMessage += "Ürün fiyatı negatif olamaz \n";
                isValid = false;
            }

            return isValid;
        }

        public async Task<List<Product>> GetAllAsync(bool trackChanges)
        {
            var products = await _unitOfWork.Products.GetAllAsync(trackChanges);
            return products;
        }

        public async Task<IEnumerable<ProductDto>> GetAllWithDetailsAsync(bool trackChanges)
        {
            var products = await _unitOfWork
                .Products
                .GetAllWithDetailsAsync(trackChanges);


            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }
    }
}
