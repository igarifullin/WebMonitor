using System.Threading;

namespace WebsiteMonitorApplication.Core.Services
{
    /// <summary>
    /// CancelleationTokenResource accessor
    /// </summary>
    public interface ICancellationTokenResourceAccessor
    {
        CancellationTokenSource GetTokenSource();
    }
}