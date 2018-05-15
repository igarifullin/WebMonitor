using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public ConfigurationService(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public async Task<int> GetDelayAsync()
        {
            var uow = _uowFactory.Create();

            var config = await uow.Get<Configuration>()
                .AsQueryable()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            return config.Interval.Milliseconds;
        }
    }
}