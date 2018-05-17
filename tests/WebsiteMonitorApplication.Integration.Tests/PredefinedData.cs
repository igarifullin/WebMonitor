using System;
using System.Collections.Generic;
using WebsiteMonitorApplication.Core.Entities;

namespace WebsiteMonitorApplication.Integration.Tests
{
    public class PredefinedData
    {
        /// <summary>
        /// Applications
        /// </summary>
        public List<Application> Applications { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        public Configuration Config { get; set; }

        public PredefinedData()
        {
            Applications = new List<Application>();
            Config = new Configuration {Interval = TimeSpan.FromSeconds(5)};
        }
    }
}