namespace WebsiteMonitorApplication.Core.Services
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}