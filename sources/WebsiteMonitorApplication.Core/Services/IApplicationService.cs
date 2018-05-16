using System;
using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IApplicationService
    {
        /// <summary>
        /// Get all application states
        /// </summary>
        /// <returns></returns>
        Task<ApplicationStateViewModel[]> GetApplicationsStatesAsync();

        /// <summary>
        /// Add new application info
        /// </summary>
        /// <param name="name">Name of application</param>
        /// <param name="description">Description</param>
        /// <param name="url">Url to application</param>
        /// <returns></returns>
        Task AddApplicationAsync(string name, string description, string url);

        /// <summary>
        /// Remove application
        /// </summary>
        /// <param name="id">Application id</param>
        /// <returns></returns>
        Task RemoveApplicationAsync(Guid id);
    }
}