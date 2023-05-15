using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.BlockingCommand.Validators
{
    public class CreateBlockingCommandValidator : AbstractValidator<CreateBlockingCommand>
    {

        public CreateBlockingCommandValidator()
        {
            RuleFor(command => command).NotNull().WithMessage("CreateMessageBlockingCommand is required.");
            RuleFor(command => command.Blacklists).NotEmpty().WithMessage("CreateMessageBlockingCommand.Blacklists is required.");
        }
    }
}
