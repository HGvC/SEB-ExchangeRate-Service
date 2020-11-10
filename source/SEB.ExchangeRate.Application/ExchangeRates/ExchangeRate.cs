using System;

namespace SEB.ExchangeRate.Application.ExchangeRates
{
    public class ExchangeRate
    {
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Unit { get; set; }
    }
}