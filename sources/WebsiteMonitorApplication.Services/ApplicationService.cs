using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _uow;

        public ApplicationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ApplicationStateViewModel[]> GetApplicationsStatesAsync()
        {
            var applications = _uow.Get<Application>().AsQueryable();
            var history = _uow.Get<ApplicationStateHistory>()
                .AsQueryable()
                .GroupBy(x => x.ApplicationId)
                .Select(x =>
                    x.OrderByDescending(z => z.Date)
                        .FirstOrDefault());

            var result = await applications.Join(history,
                    app => app.Id,
                    hist => hist.ApplicationId,
                    (app, hist) => new ApplicationStateViewModel
                    {
                        Name = app.Name,
                        CheckDate = hist.Date,
                        State = hist.State
                    })
                .ToArrayAsync();

            return result;
        }
    }
}