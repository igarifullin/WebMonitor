using System;
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
                var httpCodeCategory = (int) httpResponse.StatusCode / 100;

                switch (httpCodeCategory)
                {
                    case 1:
                    case 2:
                    case 3:
                        result.State = ApplicationState.Available;
                        break;

                    default:
                        result.State = ApplicationState.Unavailable;
                        break;
                }

                result.IsSuccess = true;
            }
            catch (HttpRequestException e)
            {
                result.IsSuccess = false;
                result.State = ApplicationState.CheckedWithError;
                result.ErrorMessage = e.Message;
                // TODO: add log
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.State = ApplicationState.CheckedWithError;
                result.ErrorMessage = e.Message;
                // TODO: add log
            }

            return result;
        }
    }
}