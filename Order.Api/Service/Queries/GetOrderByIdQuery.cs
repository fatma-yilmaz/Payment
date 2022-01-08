using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Api.Core.Validators;

namespace Order.Api.Data.Queries
{
    public class GetOrderByIdQuery :IRequest<GetOrderByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty().WithMessage($"{nameof(GetOrderByIdQuery.Id)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(GetOrderByIdQuery.Id)}_should_not_be_null")
                .Must(CustomValidators.BeAValidGuid).WithMessage($"{nameof(GetOrderByIdQuery.Id)}_should_be_in_guid_format");
        }
    }
}
