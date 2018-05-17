using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using WebsiteMonitorApplication.Data;
using WebsiteMonitorApplication.Web;

namespace WebsiteMonitorApplication.Integration.Tests
{
    public class TestApplication : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public TestApplication()
        {
            var integrationTestsPath = PlatformServices.Default.Application.ApplicationBasePath;

            // Because the test execute in folder "tests/WebsiteMonitorApplication.Integration.Tests/bin/"
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../../../../../sources/WebsiteMonitorApplication.Web"));

            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(applicationPath)
                .UseEnvironment("Testing");

            builder.ConfigureServices(c => c.AddSingleton<DatabaseSeeder, DatabaseSeeder>());

            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public ApplicationDbContext GetContext()
        {
            return _server.Host.Services.GetService<ApplicationDbContext>();
        }

        public DatabaseSeeder GetSeeder()
        {
            return _server.Host.Services.GetService<DatabaseSeeder>();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server.Dispose();
        }
    }
}