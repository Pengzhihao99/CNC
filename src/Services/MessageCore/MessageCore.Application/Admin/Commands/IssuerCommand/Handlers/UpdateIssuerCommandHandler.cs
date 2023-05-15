using MediatR;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.MessageSenderCommand.Handlers
{
    public class UpdateIssuerCommandHandler : AsyncRequestHandler<UpdateIssuerCommand>
    {
        private readonly IIssuerRepository _issuerRepository;

        public UpdateIssuerCommandHandler(IIssuerRepository issuerRepository)
        {
            _issuerRepository = issuerRepository;
        }

        protected async override Task Handle(UpdateIssuerCommand request, CancellationToken cancellationToken)
        {
            var sender = await _issuerRepository.GetByKeyAsync(request.Id);
            if (sender == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.IssuerNotFoundError, string.Format(ErrorMessage.IssuerNotFoundError, request.Id));
            }
            sender.Token = Guid.NewGuid().ToString("N");
            sender.Enabled = request.Enabled;
            sender.Remark = request.Remark;
            sender.SetUpdateOn();
            await _issuerRepository.UpdateAsync(sender);
        }
    }
}
