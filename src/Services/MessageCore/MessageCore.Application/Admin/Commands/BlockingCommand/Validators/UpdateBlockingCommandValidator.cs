using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.BlockingCommand.Validators
{
    public class UpdateBlockingCommandValidator : AbstractValidator<UpdateBlockingCommand>
    {

        public UpdateBlockingCommandValidator()
        {
            RuleFor(command => command).NotNull().WithMessage("UpdateMessageBlockingCommand is required.");
            RuleFor(command => command.Blacklists).NotEmpty().WithMessage("UpdateMessageBlockingCommand.Blacklists is required.");
        }
    }
}
