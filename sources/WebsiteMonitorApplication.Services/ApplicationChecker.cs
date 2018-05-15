using System.Net.Http;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services
{
    public class ApplicationChecker : IApplicationChecker
    {
        private readonly IHttpClient _httpClient;

        public ApplicationChecker(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CheckResult> CheckApplicationAsync(Application application)
        {
            var result = new CheckResult();

            try
            {
                var httpResponse = await _httpClient.GetAsync(application.Url);
                var httpCodeCategory = (int) httpResponse.StatusCode % 100;

                switch (httpCodeCategory)
                {
                    case 2:
                    case 3:
                        result.State = ApplicationState.Available;
                        result.IsSuccess = true;
                        break;

                    case 5:
                        result.State = ApplicationState.Unavailable;
                        result.IsSuccess = true;
                        break;
                }
            }
            catch (HttpRequestException e)
            {
                result.IsSuccess = false;
                result.State = ApplicationState.CheckedWithError;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}