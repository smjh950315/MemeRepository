using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MemeRepository.Controllers
{
    public class HomeController : MyControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWebAppConfigurations webAppConfigurations) : base(webAppConfigurations) {
            this._logger = logger;
        }

        public IActionResult Index() {
            return this.View();
        }

        public IActionResult Privacy() {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
