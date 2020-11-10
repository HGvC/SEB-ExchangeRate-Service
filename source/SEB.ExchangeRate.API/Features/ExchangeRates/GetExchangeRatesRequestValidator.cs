using FluentValidation;
using System;

namespace SEB.ExchangeRate.API.Features.ExchangeRates
{
    public class GetExchangeRatesRequestValidator : AbstractValidator<GetExchangeRatesRequest>
    {
        public GetExchangeRatesRequestValidator()
        {
            RuleFor(s => s.Date).NotEmpty();
            RuleFor(s => s.Date).Must(BeAValidDate).WithMessage("Invalid date format, example: 2012-12-12");
            RuleFor(s => s.Date).Must(BeInDatesRange).WithMessage("Date has to be less or equal to 2014-12-31");
        }

        private bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out var date);
        }

        private bool BeInDatesRange(string value)
        {
            DateTime lastDay = new DateTime(2014, 12, 31);
            DateTime.TryParse(value, out var date);

            return (date <= lastDay);
        }
    }
}