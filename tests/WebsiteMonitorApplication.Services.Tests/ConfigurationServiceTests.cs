using System;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;
using Xunit;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class ConfigurationServiceTests
    {
        [Fact]
        public async Task GetDelayAsync_TwoConfigurations_ReturnLastFromDb()
        {
            // arrange
            var configOld = new Configuration
            {
                Id = 1,
                Interval = TimeSpan.FromSeconds(10)
            };
            var configLatest = new Configuration
            {
                Id = 2,
                Interval = TimeSpan.FromSeconds(5)
            };
            var uow = new MemoryDbUoW();
            uow.Get<Configuration>().Add(configOld);
            uow.Get<Configuration>().Add(configLatest);
            uow.SaveChanges();
            
            var uowFactory = new MemoryDbUowFactory(uow);
            var sut = CreateSut(uowFactory, new AppSettings());

            // act
            int delay = await sut.GetDelayAsync();

            // assert
            Assert.NotEqual(0, delay);
            Assert.NotEqual(configOld.Interval.Seconds, delay);
            Assert.Equal(configLatest.Interval.Seconds, delay);
        }

        [Fact]
        public async Task GetDelayAsync_EmptyConfigIbDb_ReturnFromConfig()
        {
            // arrange
            IAppSettings appSettings = new AppSettings
            {
                DefaultDelay = 15
            };

            var uowFactory = new MemoryDbUowFactory();
            var sut = CreateSut(uowFactory, appSettings);

            // act
            int delay = await sut.GetDelayAsync();

            // assert
            Assert.NotEqual(0, delay);
            Assert.Equal(appSettings.DefaultDelay, delay);
        }

        private ConfigurationService CreateSut(IUnitOfWorkFactory uowFactory,
            IAppSettings appSettings)
        {
            return new ConfigurationService(uowFactory, appSettings);
        }
    }
}