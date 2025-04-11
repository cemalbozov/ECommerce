using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> GetByCondition(Expression<Func<T,bool>> expression,bool trackChanges);
        IQueryable<T> GetAll(bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
