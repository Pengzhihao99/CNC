using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.ViewModels
{
    /// <summary>
    /// 模板视图
    /// </summary>
    public class TemplateVM
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模板名
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 发送服务Id
        /// </summary>
        public string SendingServiceId { get; set; }

        /// <summary>
        /// 发送服务名字
        /// </summary>
        public string SendingServiceName { get; set; }

        /// <summary>
        /// 定时器
        /// </summary>
        public TimerType TimerType { get; set; }

        /// <summary>
        /// 唯一性策略
        /// </summary>
        public OnlyStrategyType OnlyStrategyType { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public TemplateInfo TemplateInfo { get; set; }

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

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateOn { get; set; }
    }
}
