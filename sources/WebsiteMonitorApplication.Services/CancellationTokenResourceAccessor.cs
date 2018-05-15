using System;
using System.Threading;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class CancellationTokenResourceAccessor : ICancellationTokenResourceAccessor, IDisposable
    {
        private readonly Lazy<CancellationTokenSource> _source =
            new Lazy<CancellationTokenSource>(() => new CancellationTokenSource(),
                LazyThreadSafetyMode.ExecutionAndPublication);

        public CancellationTokenSource GetTokenSource()
        {
            return _source.Value;
        }

        public void Dispose()
        {
            if (_source.IsValueCreated)
            {
                _source.Value.Dispose();
            }
        }
    }
}