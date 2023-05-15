using Framework.Domain.Core.Entities;
using MediatR;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand.Handlers
{
    public class UpdateTemplateCommandHandler : AsyncRequestHandler<UpdateTemplateCommand>
    {
        private readonly ITemplateRepository _templateRepository;

        public UpdateTemplateCommandHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        protected async override Task Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByKeyAsync(request.Id);
            if (template == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.TemplateNotFoundError, string.Format(ErrorMessage.TemplateNotFoundError, request.Id));
            }
            template.Creator = request.Creator;
            template.Enabled = request.Enabled;
            template.SetTimerType(request.TimerType);
            template.SetOnlyStrategyType(request.OnlyStrategyType);
            template.SetOnlyStrategyType(request.OnlyStrategyType);
            template.SetSendingServiceInfo(request.SendingServiceId, request.SendingServiceName);
            template.TemplateInfo.Subject= request.TemplateInfoSubject;
            template.TemplateInfo.Header  = request.TemplateInfoHeader;
            template.TemplateInfo.Content  = request.TemplateInfoContent;
            template.TemplateInfo.Footer  = request.TemplateInfoFooter;
            //template.TemplateInfo.FieldValue = request.TemplateInfoFieldValue;
            template.IssuerIds = request.IssuerIds;
            template.IssuerNames = request.IssuerNames;
            template.Remark = request.Remark ;
            template.SetUpdateOn();
            await _templateRepository.UpdateAsync(template);
        }
    }
}
