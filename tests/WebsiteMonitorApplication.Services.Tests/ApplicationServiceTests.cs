using System;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using WebsiteMonitorApplication.Core.Services;
using Xunit;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class ApplicationServiceTests
    {
        [Fact]
        public async Task GetApplicationsStatesAsync_TwoChecksInHistory_ReturnLatest()
        {
            // arrange
            var uow = new MemoryDbUoW();

            var application = new Application
            {
                Id = Guid.NewGuid(),
                LastCheckDate = null
            };

            var checkRecordOld = new ApplicationStateHistory
            {
                RecordId = Guid.NewGuid(),
                Application = application,
                ApplicationId = application.Id,
                Date = DateTime.UtcNow.AddMinutes(-10),
                State = ApplicationState.CheckedWithError
            };

            var checkRecordLatest = new ApplicationStateHistory
            {
                RecordId = Guid.NewGuid(),
                Application = application,
                ApplicationId = application.Id,
                Date = DateTime.UtcNow.AddMinutes(-2),
                State = ApplicationState.Available
            };
            uow.Get<Application>().Add(application);
            uow.Get<ApplicationStateHistory>().Add(checkRecordOld);
            uow.Get<ApplicationStateHistory>().Add(checkRecordLatest);
            uow.SaveChanges();

            var uowFactory = new MemoryDbUowFactory(uow);

            var sut = CreateSut(uowFactory);

            // act
            ApplicationStateViewModel[] viewModels = await sut.GetApplicationsStatesAsync();

            // assert
            Assert.NotEmpty(viewModels);

            var model = viewModels[0];
            Assert.NotNull(model.CheckDate);
            Assert.Equal(checkRecordLatest.Date, model.CheckDate);
            Assert.Equal(checkRecordLatest.State, model.State);
        }

        [Fact]
        public async Task GetApplicationsStatesAsync_EmptyCheckHistory_ReturnDidNotCheck()
        {

            // arrange
            var uow = new MemoryDbUoW();

            var application = new Application
            {
                Id = Guid.NewGuid(),
                LastCheckDate = null
            };
            
            uow.Get<Application>().Add(application);
            uow.SaveChanges();

            var uowFactory = new MemoryDbUowFactory(uow);

            var sut = CreateSut(uowFactory);

            // act
            ApplicationStateViewModel[] viewModels = await sut.GetApplicationsStatesAsync();

            // assert
            Assert.NotEmpty(viewModels);

            var model = viewModels[0];
            Assert.Null(model.CheckDate);
            Assert.Equal(ApplicationState.DidNotCheck, model.State);
        }

        private ApplicationService CreateSut(IUnitOfWorkFactory uowFactory)
        {
            return new ApplicationService(uowFactory);
        }
    }
}