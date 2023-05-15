using Framework.Domain.Core.Entities;
using MediatR;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Common.ValueObjects;
using MessageCore.Domain.Repositories;
using MessageCore.Domain.Repositories.ReadOnly;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand.Handlers
{
    public class CreateTemplateCommandHandler : AsyncRequestHandler<CreateTemplateCommand>
    {
        private readonly ITemplateRepository _templateRepository;

        public CreateTemplateCommandHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        protected async override Task Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
        {
            var templateInfo = new TemplateInfo(request.TemplateInfoSubject, request.TemplateInfoHeader, request.TemplateInfoContent, request.TemplateInfoFooter);

            var template = new Template(request.TemplateName, request.Creator, request.Enabled, Enumeration.FromValue<TimerType>(request.TimerType),
                Enumeration.FromValue<OnlyStrategyType>(request.OnlyStrategyType), templateInfo, request.IssuerIds, request.IssuerNames, request.Remark);
            template.SetSendingServiceInfo(request.SendingServiceId,request.SendingServiceName);
         
            await _templateRepository.AddAsync(template);
        }
    }
}
