using System;
using System.Linq;
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

        [Fact]
        public async Task AddApplicationAsync_SuccessAdded()
        {
            // arrange
            var uow = new MemoryDbUoW();
            var uowFactory = new MemoryDbUowFactory(uow);

            var name = "App-08";
            var description = "Test application";
            var url = "http://localhost:56565";

            var sut = CreateSut(uowFactory);

            // act
            await sut.AddApplicationAsync(name, description, url);

            // assert
            var app = uow.Get<Application>().Where(x => x.Name == name).FirstOrDefault();
            Assert.NotNull(app);
        }

        [Fact]
        public async Task RemoveApplicationAsync_NonExistsGuid_DoNothing()
        {

            // arrange
            Guid id = Guid.NewGuid();
            var uow = new MemoryDbUoW();
            var uowFactory = new MemoryDbUowFactory(uow);

            var sut = CreateSut(uowFactory);

            // act
            await sut.RemoveApplicationAsync(id);

            // assert
            var app = uow.Get<Application>().Where(x => x.Id == id).FirstOrDefault();
            Assert.Null(app);
        }

        [Fact]
        public async Task RemoveApplicationAsync_ExistingGuid_Remove()
        {
            // arrange
            Guid id = Guid.NewGuid();
            var uow = new MemoryDbUoW();

            var application = new Application
            {
                Id = id,
                LastCheckDate = null
            };

            uow.Get<Application>().Add(application);
            uow.SaveChanges();

            var uowFactory = new MemoryDbUowFactory(uow);

            var sut = CreateSut(uowFactory);

            // act
            await sut.RemoveApplicationAsync(id);

            // assert
            var app = uow.Get<Application>().Where(x => x.Id == id).FirstOrDefault();
            Assert.Null(app);
        }

        private ApplicationService CreateSut(IUnitOfWorkFactory uowFactory)
        {
            return new ApplicationService(uowFactory);
        }
    }
}