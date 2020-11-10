using System.Collections.Generic;
using System.Linq;

namespace SEB.ExchangeRate.API.Features.ExchangeRates
{
    internal static class GetExchangeRatesMapper
    {
        internal static IEnumerable<GetExchangeRatesResponse> ToResponse(this IEnumerable<Application.ExchangeRates.ExchangeRate> entities)
        {
            return entities.Select(entities => new GetExchangeRatesResponse
            {
                Date = entities.Date,
                CurrencyCode = entities.CurrencyCode,
                Quantity = entities.Quantity,
                Rate = entities.Rate,
                Unit = entities.Unit
            });
        }
    }
}