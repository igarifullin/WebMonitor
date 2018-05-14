using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Models;

namespace WebsiteMonitorApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationModel> Applications { get; set; }

        public DbSet<ApplicationStateHistoryModel> History { get; set; }

        public DbSet<ConfigurationModel> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationModel>().HasKey(x => x.Id);
            builder.Entity<ApplicationStateHistoryModel>().HasKey(x => x.RecordId);
            builder.Entity<ConfigurationModel>().HasKey(x => x.Id);

            builder.Entity<ApplicationModel>().HasMany(x => x.History);
            builder.Entity<ApplicationStateHistoryModel>().HasOne(x => x.Application);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
