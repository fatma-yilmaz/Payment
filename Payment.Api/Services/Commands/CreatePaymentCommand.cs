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
        public Order Order { get; set; }
        public CreatePaymentCommand(decimal amount, string currencyCode, Order order)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
            Order = order;              
        }
        public CreatePaymentCommand()
        {
                
        }
    }

    public class Order
    {
        public string ConsumerFullName { get; set; }
        public string ConsumerAddress { get; set; }
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

            RuleFor(a => a.Order)
                .NotNull().WithMessage($"{nameof(CreatePaymentCommand.Order)}_should_not_be_null");

            RuleFor(a => a.Order.ConsumerFullName)
                .NotNull().WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerFullName)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerFullName)}_should_not_be_empty")
                .MaximumLength(250).WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerFullName)}_length_should_be_less_than_250");

            RuleFor(a => a.Order.ConsumerAddress)
                .NotNull().WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerAddress)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerAddress)}_should_not_be_empty")
                .MaximumLength(1000).WithMessage($"{nameof(CreatePaymentCommand.Order.ConsumerAddress)}_length_should_be_less_than_1000");
        }
    }
}
