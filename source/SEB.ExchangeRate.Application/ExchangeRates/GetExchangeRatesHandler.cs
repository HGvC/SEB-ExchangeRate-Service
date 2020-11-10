using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SEB.ExchangeRate.Application.ExchangeRates
{
    public class GetExchangeRatesHandler : IRequestHandler<GetExchangeRates, IEnumerable<ExchangeRate>>
    {
        private readonly IAppCache _appCache;

        public GetExchangeRatesHandler(IAppCache appCache)
        {
            _appCache = appCache ?? throw new ArgumentNullException(nameof(appCache));
        }

        public Task<IEnumerable<ExchangeRate>> Handle(GetExchangeRates request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_appCache.GetOrAdd(GetCacheKey(request), () => Query(request.Date), DateTimeOffset.Now.AddHours(8)));
        }

        private IEnumerable<ExchangeRate> Query(string date)
        {
            List<ExchangeRate> requestedDay = parseExchangeRate(date);
            List<ExchangeRate> previousDay = parseExchangeRate(DateTime.Parse(date).AddDays(-1).ToShortDateString());
            List<ExchangeRate> result = new List<ExchangeRate>();

            return CalculateRate(requestedDay, previousDay).OrderByDescending(s => s.Rate);
        }

        private List<ExchangeRate> parseExchangeRate(string date)
        {
            string url = $"http://old.lb.lt//webservices/ExchangeRates/ExchangeRates.asmx/getExchangeRatesByDate?Date={date}";

            XmlDocument document = new XmlDocument();
            document.Load(url);
            XmlElement root = document.DocumentElement;
            XmlNodeList nodes = root?.SelectNodes("item");
            List<ExchangeRate> items = new List<ExchangeRate>();
            if (nodes != null)
                foreach (XmlNode node in nodes)
                {
                    var xmlElement = node["date"];
                    if (xmlElement != null)
                    {
                        ExchangeRate item = new ExchangeRate
                        {
                            Date = DateTime.Parse(node["date"].InnerText),
                            CurrencyCode = node["currency"].InnerText,
                            Quantity = XmlConvert.ToInt32(node["quantity"].InnerText),
                            Rate = XmlConvert.ToDecimal(node["rate"].InnerText),
                            Unit = node["unit"].InnerText
                        };
                        items.Add(item);
                    }
                }

            return items;
        }

        private List<ExchangeRate> CalculateRate(List<ExchangeRate> requestedDay, List<ExchangeRate> previousDay)
        {
            return requestedDay.Concat(previousDay)
                    .GroupBy(s => s.CurrencyCode)
                    .Select(x => new ExchangeRate
                    {
                        CurrencyCode = x.Key,
                        Date = x.Select(y => y.Date).FirstOrDefault(),
                        Quantity = x.Select(y => y.Quantity).FirstOrDefault(),
                        Rate = x.Select((y, i) => i == 0 ? y.Rate : -y.Rate).Sum(),
                        Unit = x.Select(y => y.Unit).FirstOrDefault()
                    })
                    .OrderByDescending(y => y.Rate)
                    .ToList();
        }

        private string GetCacheKey(GetExchangeRates request) => $"{nameof(GetExchangeRates)}-{request.Date}";
    }
}