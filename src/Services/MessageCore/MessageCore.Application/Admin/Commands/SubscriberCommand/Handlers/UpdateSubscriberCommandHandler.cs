using Framework.Infrastructure.Crosscutting.Adapter;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;
using MessageCore.Domain.Repositories;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.Common.Enum;
using Framework.Domain.Core.Entities;

namespace MessageCore.Application.Admin.Commands.SubscriberCommand.Handlers
{
    public class UpdateSubscriberCommandHandler : AsyncRequestHandler<UpdateSubscriberCommand>
    {
        private readonly ITypeAdapter _typeAdapter;
        private readonly ISubscriberRepository _subscriberRepository;

        public UpdateSubscriberCommandHandler(ITypeAdapter typeAdapter, ISubscriberRepository subscriberRepository)
        {
            _typeAdapter = typeAdapter;
            _subscriberRepository = subscriberRepository;
        }

        protected async override Task Handle(UpdateSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _subscriberRepository.GetAsync(new MatchSubscriberByIdSpecification<Subscriber>(request.Id));
            if (subscriber!=null)
            {
                subscriber.ModifyInfo(request.Email,request.Phone,request.EnterpriseWeChat, Enumeration.FromValue<SubscriberType>(request.SubscriberType), request.Enabled);
                subscriber.SetUpdateOn();
                await _subscriberRepository.UpdateAsync(subscriber, cancellationToken);
            }
        }
    }
}
