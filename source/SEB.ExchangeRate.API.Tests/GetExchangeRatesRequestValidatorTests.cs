using FluentAssertions;
using SEB.ExchangeRate.API.Features.ExchangeRates;
using System.Collections.Generic;
using Xunit;

namespace SEB.ExchangeRate.API.Tests
{
    public class GetExchangeRatesRequestValidatorTests
    {
        private readonly GetExchangeRatesRequestValidator _validator;

        public GetExchangeRatesRequestValidatorTests()
        {
            _validator = new GetExchangeRatesRequestValidator();
        }

        [Theory]
        [MemberData(nameof(ValidRequests))]
        public void Validate_WhenRequestIsValid_Passes(GetExchangeRatesRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(InValidRequests))]
        public void Validate_WhenRequestIsInvalid_Fails(GetExchangeRatesRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        public static IEnumerable<object[]> ValidRequests => new[]
       {
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "2010-11-08"
                }
            },
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "2012-11-09"
                }
            },
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "2014-12-31"
                }
            },
        };

        public static IEnumerable<object[]> InValidRequests => new[]
        {
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = ""
                }
            },
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "wadwdwa"
                }
            },
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "2015-01-01"
                }
            },
        };
    }
}