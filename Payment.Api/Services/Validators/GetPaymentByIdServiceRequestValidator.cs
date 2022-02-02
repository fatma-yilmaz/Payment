using FluentValidation;
using Payment.Api.Core.Validators;
using Payment.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Services.Validators
{

    public class GetPaymentByIdServiceRequestValidator : AbstractValidator<GetPaymentByIdServiceRequest>
    {
        public GetPaymentByIdServiceRequestValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty().WithMessage($"{nameof(GetPaymentByIdServiceRequest.Id)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(GetPaymentByIdServiceRequest.Id)}_should_not_be_null")
                .Must(CustomValidators.IsValidGuid).WithMessage($"{nameof(GetPaymentByIdServiceRequest.Id)}_should_be_in_guid_format");
        }
    }
}
