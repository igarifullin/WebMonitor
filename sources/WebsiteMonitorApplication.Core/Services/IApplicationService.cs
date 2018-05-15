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
    }
}