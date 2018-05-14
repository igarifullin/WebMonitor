using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Data
{
    public class DbUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public DbUnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> Get<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
