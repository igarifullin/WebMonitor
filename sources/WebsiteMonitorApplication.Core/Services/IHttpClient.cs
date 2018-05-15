using System.Net.Http;
using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Core.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}