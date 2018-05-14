using System;
using System.Collections.Generic;

namespace WebsiteMonitorApplication.Models
{
    /// <summary>
    /// Application model
    /// </summary>
    public class ApplicationModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Application url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of application
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Last time of check
        /// </summary>
        public DateTime? LastCheckDate { get; set; }

        /// <summary>
        /// Checks history
        /// </summary>
        public List<ApplicationStateHistoryModel> History { get; set; }
    }
}