using MediatR;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MessageCore.Application.Admin.Commands.MessageSenderCommand.Handlers
{
    public class CreateIssuerCommandHandler : AsyncRequestHandler<CreateIssuerCommand>
    {
        private readonly IIssuerRepository _issuerRepository;

        public CreateIssuerCommandHandler(IIssuerRepository issuerRepository)
        {
            _issuerRepository = issuerRepository;
        }

        protected async override Task Handle(CreateIssuerCommand request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid().ToString("N");
            var sender = new Issuer(request.Name,token,request.Enabled,request.Remark);
            //_unitOfWork.Begin();
            await _issuerRepository.AddAsync(sender);
        }
    }
}
