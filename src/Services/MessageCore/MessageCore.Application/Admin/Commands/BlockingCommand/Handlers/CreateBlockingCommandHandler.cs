using Framework.Infrastructure.UOW;
using MediatR;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.BlockingCommand.Handlers
{
    public class CreateBlockingCommandHandler : AsyncRequestHandler<CreateBlockingCommand>
    {
        private readonly IBlockingRepository _messageBlockingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBlockingCommandHandler(IBlockingRepository messageBlockingRepository, IUnitOfWork unitOfWork)
        {
            _messageBlockingRepository = messageBlockingRepository;
            _unitOfWork = unitOfWork;
        }

        protected override async Task Handle(CreateBlockingCommand request, CancellationToken cancellationToken)
        {
            var messageBlocking = new Blocking(request.TemplateName, request.Blacklists);

            //_unitOfWork.Begin();

            await _messageBlockingRepository.AddAsync(messageBlocking);

            //await _unitOfWork.CommitAsync();
        }
    }
}
