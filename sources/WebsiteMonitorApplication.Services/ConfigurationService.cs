using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly int _defaultDelay;

        public ConfigurationService(IUnitOfWorkFactory uowFactory,
            IAppSettings appSettings)
        {
            _uowFactory = uowFactory;
            _defaultDelay = appSettings.DefaultDelay;
        }

        public async Task<int> GetDelayAsync()
        {
            var uow = _uowFactory.Create();

            var config = await uow.Get<Configuration>()
                .AsQueryable()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (config != null)
            {
                return config.Interval.Seconds;
            }

            return _defaultDelay;
        }
    }
}