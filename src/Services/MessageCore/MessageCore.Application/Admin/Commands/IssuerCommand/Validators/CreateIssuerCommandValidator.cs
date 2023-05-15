using FluentValidation;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.AggregatesModels.IssuerAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.MessageSenderCommand.Validators
{
    public class CreateIssuerCommandValidator : AbstractValidator<CreateIssuerCommand>
    {
        private readonly IReadOnlyIssuerRepository _readOnlySenderRepository;

        public CreateIssuerCommandValidator(IReadOnlyIssuerRepository readOnlySenderRepository)
        {
            _readOnlySenderRepository = readOnlySenderRepository;
            RuleFor(command => command).NotNull().WithMessage("CreateIssuerCommand is required.");
            RuleFor(command => command.Name).NotEmpty().WithMessage("CreateIssuerCommand.Name is required.");
            RuleFor(command => command).MustAsync(NotExistSenderNameAsync).When(command => command != null && !string.IsNullOrWhiteSpace(command.Name)).WithMessage(command => $"ServiceCode:{command.Name} is repeat.");
        }

        private async Task<bool> NotExistSenderNameAsync(CreateIssuerCommand command, CancellationToken cancellationToken)
        {
            var result = await _readOnlySenderRepository.ExistsAsync(new MatchIssuerByNameSpecification<Issuer>(command.Name));
            return !result;
        }
    }
}
