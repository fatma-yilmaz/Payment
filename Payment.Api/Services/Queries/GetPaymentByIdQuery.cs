﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Api.Core.Validators;

namespace Payment.Api.Data.Queries
{
    public class GetPaymentByIdQuery :IRequest<GetPaymentByIdQueryResponse>
    {
        public Guid Id { get; set; }
        public GetPaymentByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetPaymentByIdQueryValidator : AbstractValidator<GetPaymentByIdQuery>
    {
        public GetPaymentByIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty().WithMessage($"{nameof(GetPaymentByIdQuery.Id)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(GetPaymentByIdQuery.Id)}_should_not_be_null")
                .Must(CustomValidators.IsValidGuid).WithMessage($"{nameof(GetPaymentByIdQuery.Id)}_should_be_in_guid_format");
        }
    }
}