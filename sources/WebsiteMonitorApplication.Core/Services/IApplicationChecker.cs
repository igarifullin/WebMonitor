using System.Threading.Tasks;
using WebsiteMonitorApplication.Core.Entities;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// Application checker
    /// Responsible for check application with http request
    /// </summary>
    public interface IApplicationChecker
    {
        Task<CheckResult> CheckApplicationAsync(Application application);
    }
}