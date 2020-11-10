using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SEB.ExchangeRate.API.Features.ExchangeRates;
using SEB.ExchangeRate.Application.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SEB.ExchangeRate.API.Tests
{
    [UnitTest]
    public class ExchangeRatesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExchangeRatesController _controller;

        public ExchangeRatesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ExchangeRatesController(_mediatorMock.Object);
        }

        [Fact]
        public void CreateController_GivenNullMediator_ThrowsArgumentNullException()
        {
            Action action = () => new ExchangeRatesController(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(RequiredParameters))]
        public async Task Get_GivenValidRequestParameters_ReturnsResponseAsync(GetExchangeRatesRequest request)
        {
            GetExchangeRates actualQuery = null;

            var expectedQuery = new GetExchangeRates(request.Date);

            var rateMock = new List<Application.ExchangeRates.ExchangeRate>
            {
                new Application.ExchangeRates.ExchangeRate
                {
                    Quantity = 100,
                    CurrencyCode = "USD",
                    Rate = 1,
                    Unit = "LTL per 1 currency unit"
                },
                new Application.ExchangeRates.ExchangeRate
                {
                    Quantity = 100,
                    CurrencyCode = "RUR",
                    Rate = 1,
                    Unit = "LTL per 1000 currency unit"
                }
            };

            var expectedResponse = new List<GetExchangeRatesResponse>
            {
                new GetExchangeRatesResponse
                {
                    Quantity = 100,
                    CurrencyCode = "USD",
                    Rate = 1,
                    Unit = "LTL per 1 currency unit"
                },
                new GetExchangeRatesResponse
                {
                    Quantity = 100,
                    CurrencyCode = "RUR",
                    Rate = 1,
                    Unit = "LTL per 1000 currency unit"
                }
            };

            _mediatorMock
                .Setup(s => s.Send(It.IsAny<GetExchangeRates>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<Application.ExchangeRates.ExchangeRate>>, CancellationToken>((query, ct) => actualQuery = (GetExchangeRates)query)
                .ReturnsAsync(rateMock);

            var response = await _controller.GetAsync(request);

            var result = response.Result.Should().BeOfType<OkObjectResult>();
            _mediatorMock.Verify(s => s.Send(It.IsAny<GetExchangeRates>(), It.IsAny<CancellationToken>()), Times.Once);
            actualQuery.Should().BeEquivalentTo(expectedQuery);
        }

        public static IEnumerable<object[]> RequiredParameters => new[]
{
            new[]
            {
                new GetExchangeRatesRequest
                {
                    Date = "2010-11-08"
                }
            }
        };
    }
}