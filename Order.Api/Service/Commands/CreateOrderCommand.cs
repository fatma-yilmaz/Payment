using FluentValidation;
using MediatR;
using Order.Api.Data.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Data.Commands
{
    public class CreateOrderCommand: IRequest<CreateOrderCommandResponse>
    {
        public string ConsumerFullName { get; set; }
        public string ConsumerAddress { get; set; }
        public CreateOrderCommand(string consumerFullName, string consumerAddress)
        {
            ConsumerFullName = consumerFullName;
            ConsumerAddress = consumerAddress;
        }
        public CreateOrderCommand()
        {

        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(a => a.ConsumerFullName)
                .NotNull().WithMessage($"{nameof(CreateOrderCommand.ConsumerFullName)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreateOrderCommand.ConsumerFullName)}_should_not_be_empty")
                .MaximumLength(250).WithMessage($"{nameof(CreateOrderCommand.ConsumerFullName)}_length_should_be_less_than_250");

            RuleFor(a => a.ConsumerAddress)
                .NotNull().WithMessage($"{nameof(CreateOrderCommand.ConsumerAddress)}_should_not_be_null")
                .NotEmpty().WithMessage($"{nameof(CreateOrderCommand.ConsumerAddress)}_should_not_be_empty")
                .MaximumLength(1000).WithMessage($"{nameof(CreateOrderCommand.ConsumerAddress)}_length_should_be_less_than_1000");
        }
    }
}
