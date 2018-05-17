using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using Xunit;

namespace WebsiteMonitorApplication.Integration.Tests
{
    public class MainTest
    {
        [Fact]
        public async Task Index_Get_ReturnsIndexHtmlPage()
        {
            // Arrange
            using (var app = new TestApplication())
            {
                var client = app.GetClient();

                // Act
                var response = await client.GetAsync("/");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Contains("<title>Home Page - WebsiteMonitorApplication.Web</title>", responseString);
            }
        }

        [Fact]
        public async Task Index_Get_ReturnsListOfApplications()
        {
            // Arrange
            using (var app = new TestApplication())
            {
                // Get predefined data
                var data = GetPredefinedData();
                // Get seeder
                var seeder = app.GetSeeder();
                // Write data to db
                await seeder.SeedAsync(data);

                var client = app.GetClient();

                // Act
                var response = await client.GetAsync("/");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                // Assert
                foreach (var application in data.Applications)
                {
                    Assert.Contains($"<h5 class=\"card-title\">{application.Name}</h5>", responseString);
                }
            }
        }

        [Fact]
        public async Task CheckDataBaseStates()
        {
            // Arrange
            using (var app = new TestApplication())
            {
                Guid failedId = Guid.NewGuid();
                // Get predefined data
                var data = GetPredefinedData(failedId);
                // Get seeder
                var seeder = app.GetSeeder();
                // Write data to db
                await seeder.SeedAsync(data);

                // Act
                // TODO: don't know how to get the total check time
                Thread.Sleep(30000);

                var dbContext = app.GetContext();

                // Assert
                foreach (var application in data.Applications)
                {
                    var checkHistoryRecord =
                        await dbContext.History.FirstOrDefaultAsync(x => x.ApplicationId == application.Id);

                    Assert.NotNull(checkHistoryRecord);
                    Assert.NotEqual(ApplicationState.DidNotCheck, checkHistoryRecord.State);

                    if (checkHistoryRecord.ApplicationId != failedId)
                    {
                        Assert.Equal(ApplicationState.Available, checkHistoryRecord.State);
                    }
                    else
                    {
                        Assert.Equal(ApplicationState.CheckedWithError, checkHistoryRecord.State);
                    }
                }
            }
        }

        private PredefinedData GetPredefinedData(Guid? failedId = null)
        {
            if (!failedId.HasValue)
            {
                failedId = Guid.NewGuid();
            }

            return new PredefinedData
            {
                Config = new Configuration
                {
                    Interval = TimeSpan.FromMinutes(10)
                },
                Applications = new List<Application>
                {
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-01",
                        Description = "Application number 01",
                        Url = "https://habrahabr.ru"
                    },
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-02",
                        Description = "Application number 02",
                        Url = "https://yandex.ru"
                    },
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-03",
                        Description = "Application number 03",
                        Url = "https://habr.com"
                    },
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-04",
                        Description = "Application number 04",
                        Url = "https://lenta.ru"
                    },
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-05",
                        Description = "Application number 05",
                        Url = "https://google.com"
                    },
                    new Application
                    {
                        Id = failedId.Value,
                        Name = "App-06",
                        Description = "Application number 06",
                        Url = "https://linkedid.com"
                    },
                    new Application
                    {
                        Id = Guid.NewGuid(),
                        Name = "App-07",
                        Description = "Application number 07",
                        Url = "https://mail.ru"
                    }
                }
            };
        }
    }
}