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
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
