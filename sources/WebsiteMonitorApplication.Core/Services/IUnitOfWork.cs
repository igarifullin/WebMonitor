using System;
using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// Pattern unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get <typeparamref name="T"/> repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> Get<T>() where T : class;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Save the changes asynchronous
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}