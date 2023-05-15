using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.MessageSenderCommand.Validators
{
    public class UpdateIssuerCommandValidator : AbstractValidator<UpdateIssuerCommand>
    {
        public UpdateIssuerCommandValidator()
        {
            RuleFor(command => command).NotNull().WithMessage("UpdateIssuerCommand is required.");
            RuleFor(command => command.Id).NotEmpty().WithMessage("UpdateIssuerCommand.id is required.");
        }
    }
}
