using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteMonitorApplication.Core.Entities
{
    /// <summary>
    /// Application model
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key]
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
        public virtual ICollection<ApplicationStateHistory> History { get; set; }

        public Application()
        {
            History = new List<ApplicationStateHistory>();
        }
    }
}