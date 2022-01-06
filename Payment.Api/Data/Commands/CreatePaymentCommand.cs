using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class CreatePaymentCommand: IRequest<CreatePaymentCommandResponse>
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(a => a.Amount)
                .NotNull().WithMessage($"{nameof(CreatePaymentCommand.Amount)}_should_not_be_null")
                .GreaterThan(0).WithMessage($"{nameof(CreatePaymentCommand.Amount)}_should_be_greater_than_zero");

            RuleFor(a => a.CurrencyCode)
                .NotEmpty().WithMessage($"{nameof(CreatePaymentCommand.CurrencyCode)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(CreatePaymentCommand.CurrencyCode)}_should_not_be_null")
                .Length(3).WithMessage($"{nameof(CreatePaymentCommand.CurrencyCode)}_length_should_be_3");
        }
    }
}
