using Microsoft.Extensions.DependencyInjection;
using WebsiteMonitorApplication.Data;

namespace WebsiteMonitorApplication.Services.Impl
{
    public static class ServiceCollectionRegistry
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<IApplicationService, ApplicationService>();
        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, DbUnitOfWork>();
        }

        public static void AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddTransient<IHttpClient, HttpClient>();
        }

        public static void AddApplicationChecker(this IServiceCollection services)
        {
            services.AddTransient<IApplicationChecker, ApplicationChecker>();
        }

        public static void AddApplicationMonitor(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationMonitorService, ApplicationMonitorService>();
        }
    }
}
