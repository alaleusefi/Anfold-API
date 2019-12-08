using System;

namespace AnfoldTask.Models
{
    public class Invoice
    {
        public string InvoiceNumber { get; }
        public string CompanyName { get; }
        public decimal InvoiceTotal { get; }

        public Invoice(string invoiceNumber, string companyName, decimal invoiceTotal)
        {
            InvoiceNumber = invoiceNumber;
            CompanyName = companyName;
            InvoiceTotal = invoiceTotal;
        }
    }
}
