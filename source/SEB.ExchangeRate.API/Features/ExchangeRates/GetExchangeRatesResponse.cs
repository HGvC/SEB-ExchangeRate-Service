using System;

namespace SEB.ExchangeRate.API.Features.ExchangeRates
{
    public class GetExchangeRatesResponse
    {
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Unit { get; set; }
    }
}