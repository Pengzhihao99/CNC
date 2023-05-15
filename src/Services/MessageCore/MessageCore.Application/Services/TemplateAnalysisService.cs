using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Services.Base;
using MessageCore.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Scriban;
using Scriban.Runtime;

namespace MessageCore.Application.Services
{
    public class TemplateAnalysisService : ITemplateAnalysisService
    {

        public async Task<TemplateResult> TemplateAnalysisAsync(TemplateInfo templateInfo, object templateFieldValue)
        {
            //主题 使用Scriban模板引擎
            var fieldValue = templateFieldValue;
            var context = new TemplateContext { MemberRenamer = member => member.Name };
            var scriptObject = new ScriptObject();
            scriptObject.Import(new { model = fieldValue });
            context.PushGlobal(scriptObject);
            var result = Template.Parse(templateInfo.Subject).Render(context);
            var subject = Template.Parse(templateInfo.Subject).Render(context);
            var header = Template.Parse(templateInfo.Header).Render(context);
            var content = Template.Parse(templateInfo.Content).Render(context);
            var footer = Template.Parse(templateInfo.Footer).Render(context);
            var htmlPage = "<html><head></head><body>" + header + content + footer + "</body></html>";

            return new TemplateResult(subject, header, content, footer, htmlPage);
        }
    }
}
