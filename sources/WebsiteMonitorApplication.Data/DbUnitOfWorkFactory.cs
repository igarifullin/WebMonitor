using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Data
{
    public class DbUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ApplicationDbContext _dbContext;

        public DbUnitOfWorkFactory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork Create()
        {
            return new DbUnitOfWork(_dbContext);
        }
    }
}
