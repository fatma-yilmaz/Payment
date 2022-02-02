using FluentValidation;
using Payment.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payment.Api.Services.Validators
{
    public class CreatePaymentServiceRequestValidator : AbstractValidator<CreatePaymentServiceRequest>
    {
        public CreatePaymentServiceRequestValidator()
        {
            RuleFor(a => a.Amount)
                .NotNull().WithMessage($"{nameof(CreatePaymentServiceRequest.Amount)}_should_not_be_null")
                .GreaterThan(0).WithMessage($"{nameof(CreatePaymentServiceRequest.Amount)}_should_be_greater_than_zero");

            RuleFor(a => a.CurrencyCode)
                .Must(IsCurrencyCodeValid).WithMessage($"{nameof(CreatePaymentServiceRequest.CurrencyCode)}_should_be_3_uppercase_character");

            RuleFor(a => a.Order)
                .NotNull().WithMessage($"{nameof(CreatePaymentServiceRequest.Order)}_should_not_be_null");

            RuleFor(a => a.Order.ConsumerFullName)
                .NotNull().WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerFullName)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerFullName)}_should_not_be_empty")
                .MaximumLength(250).WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerFullName)}_length_should_be_less_than_250");

            RuleFor(a => a.Order.ConsumerAddress)
                .NotNull().WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerAddress)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerAddress)}_should_not_be_empty")
                .MaximumLength(1000).WithMessage($"{nameof(CreatePaymentServiceRequest.Order.ConsumerAddress)}_length_should_be_less_than_1000");
        }

        private bool IsCurrencyCodeValid(string arg)
        {
            Regex regex = new Regex(@"^[A-Z]{3,3}$");
            return regex.IsMatch(arg);
        }
    }
}
