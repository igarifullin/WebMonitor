using System.Threading.Tasks;
using WebsiteMonitorApplication.Core.Entities;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IApplicationChecker
    {
        Task<CheckResult> CheckApplicationAsync(Application application);
    }
}