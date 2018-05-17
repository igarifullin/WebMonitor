using System.Threading.Tasks;
using WebsiteMonitorApplication.Data;

namespace WebsiteMonitorApplication.Integration.Tests
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(PredefinedData predefinedData)
        {
            // Add predefined configuration
            await _context.Configuration.AddAsync(predefinedData.Config);

            // Add all predefined applications
            await _context.Applications.AddRangeAsync(predefinedData.Applications);

            await _context.SaveChangesAsync();
        }
    }
}