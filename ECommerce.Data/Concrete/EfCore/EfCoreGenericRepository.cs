using ECommerce.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Concrete.EfCore
{
    public abstract class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext context;
        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }
        public void Create(TEntity entity) => 
            context.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => 
            context.Set<TEntity>().Remove(entity);

        public IQueryable<TEntity> GetAll(bool trackChanges) =>
            trackChanges ?
            context.Set<TEntity>():
            context.Set<TEntity>().AsNoTracking();

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression,
            bool trackChanges) =>
            trackChanges ?
            context.Set<TEntity>().Where(expression) :
            context.Set<TEntity>().Where(expression).AsNoTracking();

        public virtual void Update(TEntity entity) =>
            context.Set<TEntity>().Update(entity);
    }
}
