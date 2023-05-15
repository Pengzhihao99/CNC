using FluentValidation;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.AggregatesModels.TemplateAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand.Validators
{
    public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
    {
        private readonly IReadOnlyTemplateRepository _readOnlyTemplateRepository;

        public CreateTemplateCommandValidator(IReadOnlyTemplateRepository readOnlyTemplateRepository)
        {
            _readOnlyTemplateRepository = readOnlyTemplateRepository;
            RuleFor(command => command).NotNull().WithMessage("CreateTemplateCommand is required.");
            RuleFor(command => command.TemplateName).NotEmpty().WithMessage("CreateTemplateCommand.TemplateName is required.");
            RuleFor(command => command.Creator).NotEmpty().WithMessage("CreateTemplateCommand.TemplateName is Creator.");
            RuleFor(command => command.TimerType).NotEmpty().WithMessage("CreateTemplateCommand.TimerType is required.");
            RuleFor(command => command.OnlyStrategyType).NotEmpty().WithMessage("CreateTemplateCommand.OnlyStrategyType is required.");
            RuleFor(command => command.SendingServiceId).NotEmpty().WithMessage("CreateTemplateCommand.SendingServiceId is required.");
            RuleFor(command => command.SendingServiceName).NotEmpty().WithMessage("CreateTemplateCommand.SendingServiceName is required.");
            RuleFor(command => command.TemplateInfoSubject).NotEmpty().WithMessage("CreateTemplateCommand.TemplateInfoSubject is required.");
            //RuleFor(command => command.TemplateInfoHeader).NotEmpty().WithMessage("CreateTemplateCommand.TemplateInfoHeader is required.");
            RuleFor(command => command.TemplateInfoContent).NotEmpty().WithMessage("CreateTemplateCommand.TemplateInfoContent is required.");
            //RuleFor(command => command.TemplateInfoFooter).NotEmpty().WithMessage("CreateTemplateCommand.TemplateInfoFooter is required.");
            RuleFor(command => command.TemplateInfoFieldValue).NotEmpty().WithMessage("CreateTemplateCommand.TemplateInfoFieldValue is required.");
            RuleFor(command => command.IssuerIds).NotEmpty().WithMessage("CreateTemplateCommand.IssuerIds is required.");
            RuleFor(command => command).MustAsync(NotExistSenderNameAsync).When(command => command != null && !string.IsNullOrWhiteSpace(command.TemplateName)).WithMessage(command => $"TemplateName:{command.TemplateName} is repeat.");
        }

        private async Task<bool> NotExistSenderNameAsync(CreateTemplateCommand command, CancellationToken cancellationToken)
        {
            var result = await _readOnlyTemplateRepository.ExistsAsync(new MatchTemplateByTemplateNameSpecification<Template>(command.TemplateName));
            return !result;
        }
    }
}
