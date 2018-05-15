using System;

namespace WebsiteMonitorApplication.Core.Entities
{
    /// <summary>
    /// Configuration entity
    /// </summary>
    public class Configuration
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