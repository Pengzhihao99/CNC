using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services.Base
{
    /// <summary>
    /// 模板解析
    /// </summary>
    public interface ITemplateAnalysisService
    {
        public Task<TemplateResult> TemplateAnalysisAsync(TemplateInfo templateInfo, object templateFieldValue);
    }
}
