using System.Threading.Tasks;
using WebsiteMonitorApplication.Models;

namespace WebsiteMonitorApplication.Services
{
    public interface IApplicationChecker
    {
        Task<CheckResult> CheckApplicationAsync(ApplicationModel application);
    }
}