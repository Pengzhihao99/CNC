using MediatR;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Services.Base;
using MessageCore.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand.Handlers
{
    public class TemplateForTestCommandHandler : IRequestHandler<TemplateForTestCommand, TemplateResult>
    {
        private readonly ITemplateAnalysisService _templateAnalysisService;

        public TemplateForTestCommandHandler(ITemplateAnalysisService templateAnalysisService)
        {
            _templateAnalysisService = templateAnalysisService;
        }

        public async Task<TemplateResult> Handle(TemplateForTestCommand request, CancellationToken cancellationToken)
        {
            var templateInfo = new TemplateInfo(request.Subject, request.Header, request.Content, request.Footer);
            return await _templateAnalysisService.TemplateAnalysisAsync(templateInfo,request.FieldValue);
        }
    }
}
