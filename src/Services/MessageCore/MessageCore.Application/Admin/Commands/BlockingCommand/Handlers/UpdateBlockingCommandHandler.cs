using MediatR;
using MessageCore.Application.Admin.Commands.BlockingCommand;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.BlockingCommand.Handlers
{
    public class UpdateBlockingCommandHandler : AsyncRequestHandler<UpdateBlockingCommand>
    {
        private readonly IBlockingRepository _messageBlockingRepository;
        public UpdateBlockingCommandHandler(IBlockingRepository messageBlockingRepository)
        {
            _messageBlockingRepository = messageBlockingRepository;
        }

        protected async override Task Handle(UpdateBlockingCommand request, CancellationToken cancellationToken)
        {
            var messageBlocking = await _messageBlockingRepository.GetByKeyAsync(request.Id);
            if (messageBlocking == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.MessageBlockingNotFoundError, string.Format(ErrorMessage.MessageBlockingNotFoundError, request.Id));
            }

            messageBlocking.SetBlacklists(request.Blacklists);
            messageBlocking.TemplateName = request.TemplateName;
            messageBlocking.SetUpdateOn();

            await _messageBlockingRepository.UpdateAsync(messageBlocking);
        }
    }
}
