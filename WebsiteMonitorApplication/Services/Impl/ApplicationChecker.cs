using System.Net.Http;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Models;

namespace WebsiteMonitorApplication.Services.Impl
{
    public class ApplicationChecker : IApplicationChecker
    {
        private readonly IHttpClient _httpClient;

        public ApplicationChecker(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CheckResult> CheckApplicationAsync(ApplicationModel application)
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
            }

            return result;
        }
    }
}