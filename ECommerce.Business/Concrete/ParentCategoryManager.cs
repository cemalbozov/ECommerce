using ECommerce.Business.Abstract;
using ECommerce.Data.Abstract;
using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Concrete
{
    public class ParentCategoryManager : IParentCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ParentCategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Create(ParentCategory entity)
        {
            _unitOfWork.ParentCategories.Create(entity);
            _unitOfWork.Save();
        }

        public void Delete(ParentCategory entity)
        {
            _unitOfWork.ParentCategories.Delete(entity);
            _unitOfWork.Save();
        }

        public List<ParentCategory> GetAll(bool trackChanges)
        {
            return _unitOfWork.ParentCategories.GetAll(trackChanges).ToList();
        }

        public List<ParentCategory> GetAllWithCategories()
        {
            return _unitOfWork.ParentCategories.GetAllWithChildCategories();
        }

        public async Task<ParentCategory> GetByIdAsync(int id, bool trackChanges)
        {
            return await _unitOfWork.ParentCategories.GetByIdAsync(id, trackChanges);
        }

        public void Update(ParentCategory entity)
        {
            _unitOfWork.ParentCategories.Update(entity);
            _unitOfWork.Save();
        }

        public bool Validation(ParentCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
