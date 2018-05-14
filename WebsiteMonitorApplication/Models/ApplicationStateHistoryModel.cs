using System;

namespace WebsiteMonitorApplication.Models
{
    /// <summary>
    /// Application state history
    /// </summary>
    public class ApplicationStateHistoryModel
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
        public ApplicationModel Application { get; set; }
    }
}
