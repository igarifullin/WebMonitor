using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public ApplicationService(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public async Task<ApplicationStateViewModel[]> GetApplicationsStatesAsync()
        {
            var uow = _uowFactory.Create();

            var applications = uow.Get<Application>().AsQueryable();

            var history = uow.Get<ApplicationStateHistory>()
                .AsQueryable()
                .GroupBy(x => x.ApplicationId)
                .Select(x =>
                    x.OrderByDescending(z => z.Date)
                        .FirstOrDefault());

            var result = await applications
                .GroupJoin(history,
                    app => app.Id,
                    hist => hist.ApplicationId,
                    (app, hist) => new
                    {
                        App = app,
                        Histories = hist
                    })
                .SelectMany(z => z.Histories.DefaultIfEmpty(), (z, x) => new ApplicationStateViewModel
                {
                    Name = z.App.Name,
                    CheckDate = x != null ? (DateTime?)x.Date : null,
                    State = x != null ? x.State : ApplicationState.DidNotCheck
                })
                .ToArrayAsync();

            return result;
        }
    }
}