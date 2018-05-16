using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteMonitorApplication.Core.Entities;

namespace WebsiteMonitorApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ApplicationStateHistory> History { get; set; }

        public DbSet<Configuration> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Application>().HasMany(x => x.History).WithOne(x => x.Application);
            //builder.Entity<ApplicationStateHistory>().HasOne(x => x.Application).WithMany(x => x.History);

            //builder.Entity<Application>()
            //    .HasMany(x => x.History)
            //    .WithOne(x => x.Application)
            //    .HasForeignKey(x => x.ApplicationId)
            //    .HasPrincipalKey(x => x.Id);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationStateHistory>()
                .HasOne(x => x.Application)
                .WithMany(x => x.History)
                .IsRequired()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Configuration>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }
    }
}