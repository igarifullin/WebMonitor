﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationMonitorService : IApplicationMonitorService
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IApplicationChecker _checker;

        public ApplicationMonitorService(IUnitOfWorkFactory uowFactory,
            IApplicationChecker checker)
        {
            _uowFactory = uowFactory;
            _checker = checker;
        }

        public async Task CheckApplicationsAsync()
        {
            var uow = _uowFactory.Create();

            var applications = await uow.Get<Application>().AsQueryable().ToArrayAsync();

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
            var uow = _uowFactory.Create();

            var historyRecord = new ApplicationStateHistory
            {
                ApplicationId = applicationId,
                Date = DateTime.UtcNow,
                State = checkResult.State
            };

            uow.Get<ApplicationStateHistory>().Add(historyRecord);
            await uow.SaveChangesAsync();
        }
    }
}