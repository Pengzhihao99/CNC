using FluentValidation;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.SendingServiceCommand.Validators
{
    public class UpdateSendingServiceCommandValidator : AbstractValidator<UpdateSendingServiceCommand>
    {
        public UpdateSendingServiceCommandValidator()
        {
            RuleFor(command => command).NotNull().WithMessage("CreateSendingServiceCommand is required.");
            RuleFor(command => command.ServiceName).NotEmpty().WithMessage("CreateSendingServiceCommand.ServiceName is required.");
        }
    }
}
