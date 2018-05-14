using WebsiteMonitorApplication.Models;

namespace WebsiteMonitorApplication.Services
{
    /// <summary>
    /// Application check attempt result
    /// </summary>
    public class CheckResult
    {
        public ApplicationState State { get; set; }

        public bool IsSuccess { get; set; }
    }
}