using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationMonitorService : IApplicationMonitorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationChecker _checker;

        public ApplicationMonitorService(IUnitOfWork uow,
            IApplicationChecker checker)
        {
            _uow = uow;
            _checker = checker;
        }

        public async Task CheckApplicationsAsync()
        {
            var applications = await _uow.Get<Application>().AsQueryable().ToArrayAsync();

            foreach (var application in applications)
            {
                // Check application
                var checkResult = await CheckApplicationAsync(application);

                // Save result
                await SaveResultAsync(application.Id, checkResult);
            }
        }

        private Task<CheckResult> CheckApplicationAsync(Application application)
        {
            return _checker.CheckApplicationAsync(application);
        }

        private async Task SaveResultAsync(Guid applicationId, CheckResult checkResult)
        {
            var historyRecord = new ApplicationStateHistory
            {
                ApplicationId = applicationId,
                Date = DateTime.UtcNow,
                State = checkResult.State
            };

            _uow.Get<ApplicationStateHistory>().Add(historyRecord);
            await _uow.SaveChangesAsync();
        }
    }
}