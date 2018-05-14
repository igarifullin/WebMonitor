using System.ComponentModel.DataAnnotations;

namespace WebsiteMonitorApplication.Models.ApplicationViewModels
{
    public class PutApplicationViewModel
    {
        [Required]
        [Url]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        public string Description { get; set; }
    }
}