using FluentValidation;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;

namespace MessageCore.Application.Admin.Commands.SubscriberCommand.Validators
{
    public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
    {
        private readonly IReadOnlySubscriberRepository _readOnlySubscriberRepository;

        public CreateSubscriberCommandValidator(IReadOnlySubscriberRepository readOnlySubscriberRepository)
        {
            _readOnlySubscriberRepository = readOnlySubscriberRepository;
            RuleFor(command => command).NotNull().WithMessage("CreateMessageSubscriberCommand is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("CreateMessageSubscriberCommand.Name is required");
            RuleFor(command => command).MustAsync(NotExistSubscriberNameAsync)
                .When(command => command != null && !string.IsNullOrWhiteSpace(command.Name))
                .WithMessage(command => $"Name:{command.Name} is repeat.");
            RuleFor(command => command).MustAsync(NotExistSubscriberNameAndGroupAsync)
                .When(command => command != null && !string.IsNullOrWhiteSpace(command.Name) &&
                                 !string.IsNullOrWhiteSpace(command.Group)).WithMessage(command =>
                    $"Name:{command.Name} and  Group:{command.Group} is repeat.");
        }

        private async Task<bool> NotExistSubscriberNameAsync(CreateSubscriberCommand command,
            CancellationToken cancellationToken)
        {
            var result =
                await _readOnlySubscriberRepository.ExistsAsync(
                    new MatchSubscriberByEqualsNameSpecification<Subscriber>(command.Name));
            return !result;
        }

        private async Task<bool> NotExistSubscriberNameAndGroupAsync(CreateSubscriberCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _readOnlySubscriberRepository.ExistsAsync(
                new MatchSubscriberByEqualsGroupSpecification<Subscriber>(command.Group).And(
                    new MatchSubscriberByEqualsNameSpecification<Subscriber>(command.Name)));
            return !result;
        }
    }
}