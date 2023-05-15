using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand.Validators
{
    public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
    {
        public UpdateTemplateCommandValidator()
        {
            RuleFor(command => command).NotNull().WithMessage("UpdateTemplateCommand is required.");
            RuleFor(command => command.Creator).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateName is Creator.");
            //RuleFor(command => command.TemplateName).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateName is required.");
            RuleFor(command => command.TimerType).NotEmpty().WithMessage("UpdateTemplateCommand.TimerType is required.");
            RuleFor(command => command.OnlyStrategyType).NotEmpty().WithMessage("UpdateTemplateCommand.OnlyStrategyType is required.");
            RuleFor(command => command.SendingServiceId).NotEmpty().WithMessage("UpdateTemplateCommand.SendingServiceId is required.");
            RuleFor(command => command.SendingServiceName).NotEmpty().WithMessage("UpdateTemplateCommand.SendingServiceName is required.");
            RuleFor(command => command.TemplateInfoSubject).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateInfoSubject is required.");
            //RuleFor(command => command.TemplateInfoHeader).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateInfoHeader is required.");
            RuleFor(command => command.TemplateInfoContent).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateInfoContent is required.");
            //RuleFor(command => command.TemplateInfoFooter).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateInfoFooter is required.");
            //RuleFor(command => command.TemplateInfoFieldValue).NotEmpty().WithMessage("UpdateTemplateCommand.TemplateInfoFieldValue is required.");
            RuleFor(command => command.IssuerIds).NotEmpty().WithMessage("UpdateTemplateCommand.IssuerIds is required.");
        }
    }
}
