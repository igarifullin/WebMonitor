using System;
using WebsiteMonitorApplication.Core.Enums;

namespace WebsiteMonitorApplication.Core.Entities
{
    /// <summary>
    /// Application state history
    /// </summary>
    public class ApplicationStateHistory
    {
        /// <summary>
        /// Id of record
        /// </summary>
        public Guid RecordId { get; set; }

        /// <summary>
        /// Application id
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Application state
        /// </summary>
        public ApplicationState State { get; set; }

        /// <summary>
        /// Check date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Link to application by ApplicationId
        /// </summary>
        public Application Application { get; set; }
    }
}