using System.ComponentModel.DataAnnotations;

namespace WebsiteMonitorApplication.Web.Models.ApplicationViewModels
{
    /// <summary>
    /// Configuration view model
    /// </summary>
    public class ConfigurationViewModel
    {
        /// <summary>
        /// Minutes
        /// </summary>
        [Required]
        [Range(1, 10000, ErrorMessage = "Please enter valid integer Number between 1 and 10000")]
        public int Minutes { get; set; }
    }
}