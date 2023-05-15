using Framework.Application.Core.Commands;
using MediatR;
using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.TemplateCommand
{
    /// <summary>
    /// 添加模板
    /// </summary>
    public class CreateTemplateCommand : IRequest, ICommand
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 模板名
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 定时器
        /// </summary>
        public int TimerType { get; set; }

        /// <summary>
        /// 唯一性策略
        /// </summary>
        public int OnlyStrategyType { get; set; }

        /// <summary>
        /// 发送服务Id
        /// </summary>
        public string SendingServiceId { get; set; }

        /// <summary>
        /// 发送服务名字
        /// </summary>
        public string SendingServiceName { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string TemplateInfoSubject { get; set; }

        /// <summary>
        /// 模板头
        /// </summary>
        public string TemplateInfoHeader { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string TemplateInfoContent { get; set; }

        /// <summary>
        /// 模板尾
        /// </summary>
        public string TemplateInfoFooter { get; set; }

        /// <summary>
        /// 模板字段值
        /// </summary>
        public string TemplateInfoFieldValue { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public IList<string> IssuerIds { get; set; }

        /// <summary>
        ///发送者名字
        /// </summary>
        public string IssuerNames { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
