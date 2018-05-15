using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IConfigurationService
    {
        Task<int> GetDelayAsync();
    }
}