using Framework.Domain.Core.Entities;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using Scriban.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.SendingServiceCommand.Handlers
{
    public class CreateSendingServiceCommandHandler : AsyncRequestHandler<CreateSendingServiceCommand>
    {
        private readonly ISendingServiceRepository _sendingServiceRepository;

        public CreateSendingServiceCommandHandler(ISendingServiceRepository sendingServiceRepository)
        {
            _sendingServiceRepository = sendingServiceRepository;
        }

        protected async override Task Handle(CreateSendingServiceCommand request, CancellationToken cancellationToken)
        {
            var sendingService = new SendingService(request.ServiceName, Enumeration.FromValue<SendingServiceType>(request.SendingServiceType), request.Host, request.UserName, request.PassWord, request.AppKey, request.AppSecret, request.Sender, request.Enabled, request.Remark);
            await _sendingServiceRepository.AddAsync(sendingService);
        }
    }
}
