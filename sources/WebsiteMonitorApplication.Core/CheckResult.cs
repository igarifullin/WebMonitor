using WebsiteMonitorApplication.Core.Enums;

namespace WebsiteMonitorApplication.Core
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