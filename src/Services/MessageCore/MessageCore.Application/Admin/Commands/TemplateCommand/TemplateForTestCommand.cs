using Framework.Application.Core.Commands;
using MediatR;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand
{
    public class TemplateForTestCommand : IRequest<TemplateResult>, ICommand
    {
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 模板头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 模板尾
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// 模板字段值
        /// </summary>
        public object FieldValue { get; set; }
    }
}
