using AnfoldTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AnfoldTask.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetInvoiceReports();
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly IConfiguration config;
        private readonly IAuthenticationService authentication;
        private readonly HttpClient httpClient;
        private readonly string apiAddress;

        public InvoiceService(IConfiguration config, IAuthenticationService authentication)
        {
            this.config = config;
            this.authentication = authentication;
            apiAddress = config.GetValue<string>("ApiSettings:Url") + config.GetValue<string>("ApiSettings:InvoiceReport");
            httpClient = new HttpClient();

        }

        public async Task<IEnumerable<Invoice>> GetInvoiceReports()
        {
            var body = new
            {
                StartDate = "2018-01-01",
                EndDate = "2019-12-31",
                ExportedFilter = "NoFilter",
                InvoiceReportType = "ClientInvoices",
                InvoiceStatusFilters = new[] { "Draft", "Approved", "Paid" }
            };
            var bodyJson = JsonConvert.SerializeObject(body);
            var requestBody = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            string token = string.Empty;
            if (authentication.HasToken)
                token = authentication.GetToken();
            else throw new InvalidOperationException("Please authenticate first.");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync(apiAddress, requestBody);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException("Failed at API endpoint");
            //throw new HttpRequestException("Authentication faild at API endpoint");

            var invoiceReport = await response.Content.ReadAsStringAsync();
            var deserializedInvoices = JsonConvert.DeserializeObject<string[][]>(invoiceReport);
            var result = deserializedInvoices.Select(a => new Invoice(a[0], a[1], decimal.Parse(a[2])));
            return result;
        }
    }
}
