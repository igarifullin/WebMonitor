using System;
using WebsiteMonitorApplication.Core.Enums;

namespace WebsiteMonitorApplication.Core
{
    /// <summary>
    /// Application state view model
    /// </summary>
    public class ApplicationStateViewModel
    {
        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Last state
        /// </summary>
        public ApplicationState State { get; set; }

        /// <summary>
        /// Last check date
        /// </summary>
        public DateTime? CheckDate { get; set; }
    }
}