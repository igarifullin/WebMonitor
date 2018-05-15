using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IApplicationMonitorService
    {
        /// <summary>
        /// Check all aplications asynchronous
        /// </summary>
        /// <returns></returns>
        Task CheckApplicationsAsync();
    }
}