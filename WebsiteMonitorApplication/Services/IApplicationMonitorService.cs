using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Services
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
