using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// Application monitor
    /// Responsible for:
    /// - get all aplications
    /// - check each application with the checker
    /// - log check result
    /// </summary>
    public interface IApplicationMonitorService
    {
        /// <summary>
        /// Check all aplications asynchronous
        /// </summary>
        /// <returns></returns>
        Task CheckApplicationsAsync();
    }
}