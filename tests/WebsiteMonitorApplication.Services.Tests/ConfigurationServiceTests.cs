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
            TimeSpan delay = await sut.GetDelayAsync();

            // assert
            Assert.NotEqual(0, delay.Seconds);
            Assert.NotEqual(configOld.Interval.Seconds, delay.Seconds);
            Assert.Equal(configLatest.Interval.Seconds, delay.Seconds);
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
            TimeSpan delay = await sut.GetDelayAsync();

            // assert
            Assert.NotEqual(0, delay.Seconds);
            Assert.Equal(appSettings.DefaultDelay, delay.Seconds);
        }

        private ConfigurationService CreateSut(IUnitOfWorkFactory uowFactory,
            IAppSettings appSettings)
        {
            return new ConfigurationService(uowFactory, appSettings);
        }
    }
}