using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEB.ExchangeRate.Application.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEB.ExchangeRate.API.Features.ExchangeRates
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRatesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetExchangeRatesResponse>>> GetAsync([FromQuery] GetExchangeRatesRequest request)
        {
            var query = new GetExchangeRates(request.Date);

            var result = await _mediator.Send(query);

            var response = result.ToResponse();

            return Ok(response);
        }
    }
}