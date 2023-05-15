using Framework.Domain.Core.Entities;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Common.ValueObjects;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.TemplateAggregate
{
    /// <summary>
    /// 模板信息管理
    /// </summary>
    public class Template : AggregateRoot
    {
        /// <summary>
        /// 模板名(需要唯一)
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
        public string SendingServiceId { get;private set; }

        /// <summary>
        /// 发送服务名字
        /// </summary>
        public string SendingServiceName { get; private set; }

        /// <summary>
        /// 定时器
        /// </summary>
        public TimerType TimerType { get; private set; }

        /// <summary>
        /// 唯一性策略
        /// </summary>
        public OnlyStrategyType OnlyStrategyType { get; private set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public TemplateInfo TemplateInfo { get; set; }

        /// <summary>
        /// 发送者Id集合
        /// </summary>
        public IList<string> IssuerIds { get; set; }

        /// <summary>
        /// 发送者名字 用于前端展示 主要作用在于字段IssuerIds上
        /// </summary>
        public string IssuerNames { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Template(string templateName, string creator, bool enabled, TimerType timerType, OnlyStrategyType onlyStrategyType, 
            TemplateInfo templateInfo, IList<string> issuerIds, string senderNames, string remark)
        {

            TemplateName = templateName;
            Creator = creator;
            TimerType = timerType;
            OnlyStrategyType = onlyStrategyType;
            Enabled = enabled;
            TemplateInfo = templateInfo;
            IssuerIds = issuerIds;
            Remark = remark;
            IssuerNames = senderNames;
        }

        public void SetSendingServiceInfo(string sendingServiceId, string sendingServiceName)
        {
            SendingServiceId = sendingServiceId;
            SendingServiceName = sendingServiceName;
        }

        public void SetTimerType(int timerType)
        {
            TimerType = Enumeration.FromValue<TimerType>(timerType);
        }

        public void SetOnlyStrategyType(int onlyStrategyType)
        {
            OnlyStrategyType = Enumeration.FromValue<OnlyStrategyType>(onlyStrategyType);
        }
    }
}
