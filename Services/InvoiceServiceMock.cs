using AnfoldTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnfoldTask.Services
{
 
    public class InvoiceServiceMock :IInvoiceService
    {
        private readonly List<Invoice> invoices = new List<Invoice> {
            new Invoice("1", "Anfold Software", 1200M),
            new Invoice("2", "Cisco", 1200M),
            new Invoice("3", "Juniper", 1200M),
            new Invoice("4", "Microsoft", 1200M),
            new Invoice("5", "Oracle", 1200M),
            new Invoice("6", "Smiths", 1200M),
            new Invoice("7", "Apple", 1200M),
            new Invoice("8", "Amazon", 1200M),
            new Invoice("9", "Fortinet", 1200M),
            new Invoice("10", "Huawei", 1200M),
            new Invoice("11", "Ford", 1200M),
            new Invoice("12", "Mitsubishi", 1200M),
            new Invoice("13", "Facebook", 1200M),
        };



        [HttpPost]
        public async Task<IEnumerable<Invoice>> GetInvoiceReports()
        {
            var invoiceArray = invoices.Select(i => new[] { i.InvoiceNumber, i.CompanyName, i.InvoiceTotal.ToString() }).ToArray();
            var serialisedInvoiceArray = JsonConvert.SerializeObject(invoiceArray);
            var deserializeInvoices = (IEnumerable<Invoice>)JsonConvert.DeserializeObject(serialisedInvoiceArray, typeof(IEnumerable<Invoice>));
            //typeof(Invoice).GetProperties(BindingFlags.);
            //var invoiceArray = invoices.To2DArray(x=>x.InvoiceNumber.ToString(), x=>x.CompanyName.ToString(), x=>x.InvoiceTotal.ToString());
            return deserializeInvoices;
        }
      
    }
}
