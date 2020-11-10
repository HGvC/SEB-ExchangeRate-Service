using FluentAssertions;
using LazyCache;
using SEB.ExchangeRate.Application.ExchangeRates;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SEB.ExchangeRate.Application.Tests
{
    [UnitTest]
    public class GetExchangeRatesHandlerTests
    {
        private readonly GetExchangeRatesHandler _handler;
        private readonly IAppCache _appCache;

        public GetExchangeRatesHandlerTests()
        {
            _appCache = new CachingService();
            _handler = new GetExchangeRatesHandler(_appCache);
        }

        [Fact]
        public async Task Handle_GivenDateWasFound_ReturnsRates()
        {
            var request = new GetExchangeRates("2012-01-01");

            var result = await _handler.Handle(request, default);

            result.Should().NotBeNullOrEmpty();
            result.Should().OnlyContain(s => s.Date == DateTime.Parse("2012-01-01"));
        }

        [Fact]
        public async Task Handle_GivenDateWasNotFound_ReturnsEmptyCollection()
        {
            var request = new GetExchangeRates("2018-01-01");

            var result = await _handler.Handle(request, default);

            result.Should().BeEmpty();
        }
    }
}