using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        IQueryable<T> Entities { get; }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get quearyable
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);
    }
}