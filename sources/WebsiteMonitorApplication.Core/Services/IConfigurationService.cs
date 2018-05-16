using System;
using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IConfigurationService
    {
        Task<TimeSpan> GetDelayAsync();

        Task ChangeDelayAsync(TimeSpan time);
    }
}