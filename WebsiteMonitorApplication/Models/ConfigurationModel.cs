using System;

namespace WebsiteMonitorApplication.Models
{
    /// <summary>
    /// Configuration model
    /// </summary>
    public class ConfigurationModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Interval between checking
        /// </summary>
        public TimeSpan Interval { get; set; }
    }
}