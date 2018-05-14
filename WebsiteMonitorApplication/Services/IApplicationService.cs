using System.Threading.Tasks;
using WebsiteMonitorApplication.Models.ApplicationViewModels;

namespace WebsiteMonitorApplication.Services
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