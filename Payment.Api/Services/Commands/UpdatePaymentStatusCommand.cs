using FluentValidation;
using MediatR;
using Payment.Api.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class UpdatePaymentStatusCommand : IRequest<UpdatePaymentStatusCommandResponse>
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
        public UpdatePaymentStatusCommand(Guid paymentId, String status)
        {
            PaymentId = paymentId;
            Status = status;
        }
    }
    public class UpdatePaymentStatusCommandValidator : AbstractValidator<UpdatePaymentStatusCommand>
    {
        public UpdatePaymentStatusCommandValidator()
        {
            RuleFor(a => a.PaymentId)
                .NotEmpty().WithMessage($"{nameof(UpdatePaymentStatusCommand.PaymentId)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(UpdatePaymentStatusCommand.PaymentId)}_should_not_be_null")
                .Must(CustomValidators.IsValidGuid).WithMessage($"{nameof(UpdatePaymentStatusCommand.PaymentId)}_should_be_in_guid_format");

            RuleFor(a => a.Status)
                .NotEmpty().WithMessage($"{nameof(UpdatePaymentStatusCommand.Status)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(UpdatePaymentStatusCommand.Status)}_should_not_be_null");
        }
    }
}
