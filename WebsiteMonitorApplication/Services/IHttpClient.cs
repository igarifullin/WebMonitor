using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebsiteMonitorApplication.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}