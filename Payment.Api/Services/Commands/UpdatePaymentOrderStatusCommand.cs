using FluentValidation;
using MediatR;
using Payment.Api.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class UpdatePaymentOrderStatusCommand : IRequest<UpdatePaymentOrderStatusCommandResponse>
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public UpdatePaymentOrderStatusCommand(Guid paymentId, Guid orderId)
        {
            PaymentId = paymentId;
            OrderId = orderId;
        }
    }

    public class UpdatePaymentOrderStatusCommandValidator : AbstractValidator<UpdatePaymentOrderStatusCommand>
    {
        public UpdatePaymentOrderStatusCommandValidator()
        {
            RuleFor(a => a.PaymentId)
                .NotEmpty().WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.PaymentId)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.PaymentId)}_should_not_be_null")
                .Must(CustomValidators.IsValidGuid).WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.PaymentId)}_should_be_in_guid_format");
            
            RuleFor(a => a.OrderId)
                .NotEmpty().WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.OrderId)}_should_not_be_empty")
                .NotNull().WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.OrderId)}_should_not_be_null")
                .Must(CustomValidators.IsValidGuid).WithMessage($"{nameof(UpdatePaymentOrderStatusCommand.OrderId)}_should_be_in_guid_format");
        }
    }
}
