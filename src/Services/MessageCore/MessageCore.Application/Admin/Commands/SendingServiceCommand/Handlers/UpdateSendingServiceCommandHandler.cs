using Framework.Domain.Core.Entities;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.SendingServiceCommand.Handlers
{
    public class UpdateSendingServiceCommandHandler : AsyncRequestHandler<UpdateSendingServiceCommand>
    {
        private readonly ISendingServiceRepository _sendingServiceRepository;

        public UpdateSendingServiceCommandHandler(ISendingServiceRepository sendingServiceRepository)
        {
            _sendingServiceRepository = sendingServiceRepository;
        }

        protected async override Task Handle(UpdateSendingServiceCommand request, CancellationToken cancellationToken)
        {
            var sendingService = await _sendingServiceRepository.GetByKeyAsync(request.Id);
            if (sendingService == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.MessageSendingServiceNotFoundError, string.Format(ErrorMessage.MessageSendingServiceNotFoundError, request.Id));
            }

            sendingService.ModifyInfo(request.ServiceName, Enumeration.FromValue<SendingServiceType>(request.SendingServiceType), request.Host, request.UserName, request.PassWord, request.AppKey, request.AppSecret, request.Sender, request.Enabled, request.Remark);
            sendingService.SetUpdateOn();
            await _sendingServiceRepository.UpdateAsync(sendingService);
        }
    }
}
