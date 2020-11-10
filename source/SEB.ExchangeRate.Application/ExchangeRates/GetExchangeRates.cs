using MediatR;
using System.Collections.Generic;

namespace SEB.ExchangeRate.Application.ExchangeRates
{
    public class GetExchangeRates : IRequest<IEnumerable<ExchangeRate>>
    {
        public string Date { get; }

        public GetExchangeRates(string date)
        {
            Date = date;
        }
    }
}