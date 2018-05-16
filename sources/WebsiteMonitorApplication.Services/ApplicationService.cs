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
                    Description = z.App.Description,
                    Url = z.App.Url,
                    CheckDate = x != null ? (DateTime?)x.Date : null,
                    State = x != null ? x.State : ApplicationState.DidNotCheck
                })
                .ToArrayAsync();

            return result;
        }

        public async Task AddApplicationAsync(string name, string description, string url)
        {
            var application = new Application
            {
                Id = Guid.NewGuid(),
                Description = description,
                Name = name,
                Url = url
            };

            var uow = _uowFactory.Create();
            uow.Get<Application>().Add(application);
            await uow.SaveChangesAsync();
        }

        public async Task RemoveApplicationAsync(Guid id)
        {
            var uow = _uowFactory.Create();
            var application = await uow.Get<Application>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (application != null)
            {
                uow.Get<Application>().Remove(application);
                await uow.SaveChangesAsync();
            }
        }
    }
}