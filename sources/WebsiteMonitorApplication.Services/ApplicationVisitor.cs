using System.Threading;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationVisitor : IApplicationVisitor
    {
        //private readonly ICancellationTokenResourceAccessor _cancellationTokenResourceAccessor;
        private readonly IApplicationMonitorService _monitorService;
        private readonly IConfigurationService _configurationService;

        public ApplicationVisitor(//ICancellationTokenResourceAccessor cancellationTokenResourceAccessor,
            IApplicationMonitorService monitorService,
            IConfigurationService configurationService)
        {
            //_cancellationTokenResourceAccessor = cancellationTokenResourceAccessor;
            _monitorService = monitorService;
            _configurationService = configurationService;
        }

        public async Task StartAsync()
        {
            await _monitorService.CheckApplicationsAsync();

            await CompleteAsync();
        }

        public async Task CompleteAsync()
        {
            var delay = await GetDelayAsync();
            Thread.Sleep(delay * 1000);

            //var tokenResource = _cancellationTokenResourceAccessor.GetTokenSource();
            //if (!tokenResource.IsCancellationRequested)
            //{
                await StartAsync();
            //}
        }

        private Task<int> GetDelayAsync()
        {
            return _configurationService.GetDelayAsync();
        }
    }
}