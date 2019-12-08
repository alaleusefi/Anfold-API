using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnfoldTask.Models;
using Microsoft.Extensions.Configuration;
using AnfoldTask.Services;

namespace AnfoldTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IAuthenticationService authenticationService;
        private readonly IInvoiceService invoiceService;

        public HomeController(ILogger<HomeController> logger, IAuthenticationService authenticationService, IInvoiceService invoiceService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
            this.invoiceService = invoiceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Connect(Credential credential)
        {
            if (authenticationService.HasToken == false)
                await authenticationService.Authenticate(credential);
            var reports = await invoiceService.GetInvoiceReports();
            return View("~/Views/Home/Invoice.cshtml", reports);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

