using System;
using System.Linq;
using System.Linq.Expressions;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services.Tests
{
    internal interface IMemoryRepository
    {
        void Submit();
    }

    public class MemoryRepository<T> : IRepository<T>, IMemoryRepository where T : class
    {
        private readonly MemoryQueryable<T> _queryable = new MemoryQueryable<T>();
        
        public IQueryable<T> Entities { get; }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return AsQueryable().Where(predicate);
        }

        public IQueryable<T> AsQueryable()
        {
            return _queryable.AsQueryable();
        }

        public void Remove(T entity)
        {
            _queryable.Remove(entity);
        }

        public void Add(T entity)
        {
            _queryable.Add(entity);
        }
        
        #region IMemoryRepository
        public void Submit()
        {
            _queryable.Submit();
        }
        #endregion
    }
}