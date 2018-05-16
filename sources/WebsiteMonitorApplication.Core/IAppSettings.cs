namespace WebsiteMonitorApplication.Core
{
    public interface IAppSettings
    {
        int DefaultDelay { get; }
    }

    public class AppSettings : IAppSettings
    {
        public int DefaultDelay { get; set; }
    }
}