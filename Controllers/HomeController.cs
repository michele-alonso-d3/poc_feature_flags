using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using poc_feature_flags.Models;

namespace poc_feature_flags.Controllers
{
     public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("appConfiguration")]
        [HttpGet]
        public JsonResult Configuration()
        {
            var lstConfigs = _configuration.AsEnumerable().ToList();

            List<FeatureFlag> lstFeatureFlags = new List<FeatureFlag>();

            foreach (var item in lstConfigs)
            {
                if (item.Key.Contains(".appconfig.featureflag"))
                    lstFeatureFlags.Add(JsonConvert.DeserializeObject<FeatureFlag>(item.Value));
            }

            return Json(lstFeatureFlags);
        }
    }
}
