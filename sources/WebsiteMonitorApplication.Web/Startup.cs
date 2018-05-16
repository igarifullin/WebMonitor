using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Services;
using WebsiteMonitorApplication.Data;
using WebsiteMonitorApplication.Services;
using WebsiteMonitorApplication.Web.Services;
using WebsiteMonitorApplication.Web.Services.Impl;

namespace WebsiteMonitorApplication.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
        {
            CurrentEnvironment = currentEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment CurrentEnvironment { get; }

        private static Thread _backgroundThread;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsEnvironment("testing"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestingDB"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                    ServiceLifetime.Transient,
                    ServiceLifetime.Transient);
            }

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddUnitOfWork();
            services.AddApplicationService();
            services.AddCustomHttpClient();
            services.AddApplicationChecker();
            services.AddApplicationMonitor();
            services.AddConfigurationService();
            services.AddApplicationVisitor();
            services.AddCancellationTokenResourceAccessor();

            services.AddMvc();

            IAppSettings appSettings = new AppSettings();
            Configuration.GetSection("appSettings").Bind(appSettings);
            services.AddSingleton<IAppSettings>(appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var provider = app.ApplicationServices;
                var visitor = (IApplicationVisitor)provider.GetService(typeof(IApplicationVisitor));
                _backgroundThread = new Thread(() => visitor.StartAsync());
                _backgroundThread.Start();
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                if (_backgroundThread != null)
                {
                    if (_backgroundThread.IsAlive)
                    {
                        _backgroundThread.Abort();
                    }
                }
            });
        }
    }
}
