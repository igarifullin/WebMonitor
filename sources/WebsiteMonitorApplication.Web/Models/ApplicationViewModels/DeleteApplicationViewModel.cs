using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteMonitorApplication.Web.Models.ApplicationViewModels
{
    public class DeleteApplicationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public Guid Id { get; set; }
    }
}
