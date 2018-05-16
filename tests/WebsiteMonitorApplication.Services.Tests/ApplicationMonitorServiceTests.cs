using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using WebsiteMonitorApplication.Core.Services;
using Xunit;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class ApplicationMonitorServiceTests
    {
        [Theory]
        [InlineData(ApplicationState.Unavailable)]
        [InlineData(ApplicationState.Available)]
        [InlineData(ApplicationState.CheckedWithError)]
        public async Task CheckApplicationsAsync_SuccessSaveResult(ApplicationState state)
        {
            // arrange
            var application = new Application
            {
                Id = Guid.NewGuid(),
                LastCheckDate = null
            };
            var uow = new MemoryDbUoW();
            uow.Get<Application>().Add(application);
            uow.SaveChanges();

            var uowFactory = new MemoryDbUowFactory(uow);
            var applicationCheckerMock = new Mock<IApplicationChecker>();
            applicationCheckerMock.Setup(x => x.CheckApplicationAsync(It.IsAny<Application>())).ReturnsAsync(
                new CheckResult
                {
                    State = state
                });

            var sut = CreateSut(uowFactory, applicationCheckerMock.Object);

            // act
            await sut.CheckApplicationsAsync();

            // assert
            var history = uow.Get<ApplicationStateHistory>().Where(x => x.ApplicationId == application.Id).FirstOrDefault();
            Assert.NotNull(history);
            Assert.Equal(state, history.State);
            Assert.NotEqual(DateTime.MinValue, history.Date);

            var applicationFromDb = uow.Get<Application>().Where(x => x.Id == application.Id).FirstOrDefault();
            Assert.NotNull(applicationFromDb);
            Assert.NotNull(applicationFromDb.LastCheckDate);
            Assert.Equal(history.Date, applicationFromDb.LastCheckDate);
        }

        private ApplicationMonitorService CreateSut(IUnitOfWorkFactory uowFactory,
            IApplicationChecker applicationChecker)
        {
            return new ApplicationMonitorService(uowFactory, applicationChecker);
        }
    }
}