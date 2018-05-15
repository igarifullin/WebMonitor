﻿using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// Responsible for the periodic applications check
    /// </summary>
    public interface IApplicationVisitor
    {
        Task StartAsync();
    }
}