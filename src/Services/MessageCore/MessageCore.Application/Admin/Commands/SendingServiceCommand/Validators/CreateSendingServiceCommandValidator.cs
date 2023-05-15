using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Domain.AggregatesModels.IssuerAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.Repositories;
using FluentValidation;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate.Specifications;
using System.Collections;

namespace MessageCore.Application.Admin.Commands.SendingServiceCommand.Validators
{
    public class CreateSendingServiceCommandValidator : AbstractValidator<CreateSendingServiceCommand>
    {
        private readonly ISendingServiceRepository _sendingServiceRepository;

        public CreateSendingServiceCommandValidator(ISendingServiceRepository sendingServiceRepository)
        {
            _sendingServiceRepository = sendingServiceRepository;
            RuleFor(command => command).NotNull().WithMessage("CreateSendingServiceCommand is required.");
            RuleFor(command => command.ServiceName).NotEmpty().WithMessage("CreateSendingServiceCommand.ServiceName is required.");
            RuleFor(command => command).MustAsync(NotExistServiceNameAsync).When(command => command != null && !string.IsNullOrWhiteSpace(command.ServiceName)).WithMessage(command => $"ServiceName:{command.ServiceName} is repeat.");
        }

        private async Task<bool> NotExistServiceNameAsync(CreateSendingServiceCommand command, CancellationToken cancellationToken)
        {
            var result = await _sendingServiceRepository.ExistsAsync(new MatchSendingServiceByServiceNameSpecification<SendingService>(command.ServiceName));
            return !result;
        }
    }
}
