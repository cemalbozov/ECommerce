using ECommerce.Business.Abstract;
using ECommerce.Data.Abstract;
using ECommerce.Entity.Exceptions;
using ECommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Create(Category entity)
        {
            _unitOfWork.Categories.Create(entity);
            _unitOfWork.Save();
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            _unitOfWork.Categories.Create(entity);
            await _unitOfWork.SaveAsync();

            return entity;
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _unitOfWork.Categories.DeleteFromCategory(productId,categoryId);
            _unitOfWork.Save();
        }

        public List<Category> GetAll()
        {
            return  _unitOfWork.Categories.GetAll(false).ToList();
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool trackChanges)
        {
            return await _unitOfWork.Categories.GetAll(trackChanges)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id, bool trackChanges)
        {
            var category = await _unitOfWork
                .Categories
                .GetByIdAsync(id, trackChanges);

            if (category is null)
                throw new CategoryNotFoundException(id);

            return category;
        }

        public Category GetByIdWithParentCAndProducts(int id)
        {
            return _unitOfWork.Categories.GetByIdWithParentCAndProducts(id);
        }

        public void Update(Category entity)
        {
            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Save();
        }

        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
