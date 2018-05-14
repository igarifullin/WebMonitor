using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteMonitorApplication.Models.ApplicationViewModels
{
    public class DeleteApplicationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public Guid Id { get; set; }
    }
}
