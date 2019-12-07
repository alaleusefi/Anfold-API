using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnfoldTask.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace AnfoldTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Connect()
        {
            var baseAddress = _config.GetValue<string>("ApiSettings:Url");
            var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseAddress);
            //var requestBodyObj = new { Email = "demo@test.anfold.com", Password = "Anfold123!", AccountName = "demo" };
            //var requestBodyJson = JsonConvert.SerializeObject(requestBodyObj);

            var requestBody = new FormUrlEncodedContent(new[]
{
    new KeyValuePair<string, string>("Email", "demo@test.anfold.com"),
    new KeyValuePair<string, string>("Password", "Anfold123!"),
    new KeyValuePair<string, string>("AccountName", "demo"),
});
            //var requestBody = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            var responseTask = httpClient.PostAsync(baseAddress + "/token", requestBody);
            var response = await responseTask;
            var token = await response.Content.ReadAsStringAsync();
            return token.ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
