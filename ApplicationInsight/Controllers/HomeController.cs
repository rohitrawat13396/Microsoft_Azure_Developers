using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApplicationInsight.Models;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;

namespace ApplicationInsight.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
        	
	    var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = "<your instrumentaion key>";

            var client = new TelemetryClient(config);

            // Text can go using trackevent
            client.TrackEvent("Hey! I am in Home page");

            // Redireting to other page can be done using trackpageview
            // Metric info can go using TrackMetric

            client.TrackMetric("LoadTime", 100);

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
    }
}
