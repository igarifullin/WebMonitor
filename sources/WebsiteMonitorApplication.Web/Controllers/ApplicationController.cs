using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteMonitorApplication.Core.Services;
using WebsiteMonitorApplication.Web.Models.ApplicationViewModels;

namespace WebsiteMonitorApplication.Web.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IConfigurationService _configurationService;

        public ApplicationController(IApplicationService applicationService,
            IConfigurationService configurationService)
        {
            _applicationService = applicationService;
            _configurationService = configurationService;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _applicationService.GetApplicationsStatesAsync();
            return View(applications);
        }

        [HttpGet]
        public IActionResult NewApplication()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewApplication(PutApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _applicationService.AddApplicationAsync(model.Name, model.Description, model.Url);

                    return RedirectToAction(nameof(ApplicationController.Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Internal server error");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Configuration()
        {
            ConfigurationViewModel model = new ConfigurationViewModel();
            TimeSpan delay = await _configurationService.GetDelayAsync();
            model.Minutes = delay.Minutes;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Configuration(ConfigurationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _configurationService.ChangeDelayAsync(TimeSpan.FromMinutes(model.Minutes));

                    return RedirectToAction(nameof(ApplicationController.Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Internal server error");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _applicationService.RemoveApplicationAsync(id);

            return RedirectToAction(nameof(ApplicationController.Index));
        }
    }
}