using System;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var webhost = BuildWebHost(args);
            var provider = webhost.Services;
            var visitor = (IApplicationVisitor)provider.GetService(typeof(IApplicationVisitor));
            var backgroundThread = new Thread(() => visitor.StartAsync());
            backgroundThread.Start();

            webhost.Run();

            backgroundThread.Abort();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
