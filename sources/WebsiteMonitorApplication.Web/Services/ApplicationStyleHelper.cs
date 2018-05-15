using WebsiteMonitorApplication.Core.Enums;

namespace WebsiteMonitorApplication.Web.Services
{
    public static class ApplicationStyleHelper
    {
        public static string GetStyleForApplication(ApplicationState state)
        {
            switch (state)
            {
                case ApplicationState.Available:
                    return "text-white bg-success";
                case ApplicationState.Unavailable:
                    return "text-white bg-danger";
                case ApplicationState.CheckedWithError:
                    return "text-white bg-warning";
                case ApplicationState.DidNotCheck:
                    return "text-black bg-light";
                default:
                    return "text-black bg-secondary";
            }
        }

        public static string GetStatus(ApplicationState state)
        {
            switch (state)
            {
                case ApplicationState.Available:
                    return "Available";
                case ApplicationState.Unavailable:
                    return "Unavailable";
                case ApplicationState.CheckedWithError:
                    return "Checked with error";
                case ApplicationState.DidNotCheck:
                    return "Didn't checked yet";
                default:
                    return "Unknown";
            }
        }
    }
}