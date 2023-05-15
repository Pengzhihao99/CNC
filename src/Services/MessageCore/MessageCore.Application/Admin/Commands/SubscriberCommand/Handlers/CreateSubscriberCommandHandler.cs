using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Entities;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.Repositories;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Domain.Common.Enum;

namespace MessageCore.Application.Admin.Commands.SubscriberCommand.Handlers
{
    public class CreateSubscriberCommandHandler : AsyncRequestHandler<CreateSubscriberCommand>
    {
        private readonly ITypeAdapter _typeAdapter;
        private readonly ISubscriberRepository _subscriberRepository;

        public CreateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, ITypeAdapter typeAdapter)
        {
            _subscriberRepository = subscriberRepository;
            _typeAdapter = typeAdapter;
        }

        protected async override Task Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = new Subscriber(request.Name,request.Email,request.Phone,request.EnterpriseWeChat, Enumeration.FromValue<SubscriberType>(request.SubscriberType), request.Group,request.Enabled);
            await _subscriberRepository.AddAsync(subscriber);
        }
    }
}
