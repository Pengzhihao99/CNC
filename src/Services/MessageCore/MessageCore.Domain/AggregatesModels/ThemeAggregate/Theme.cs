using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.ThemeAggregate
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public class Theme : AggregateRoot
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 订阅者类型
        /// </summary>
        public SubscriberType SubscriberType { get; private set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 信息模板绑定
        /// </summary>
        public IList<string> TemplateIds { get; private set; }

        /// <summary>
        /// 订阅者信息
        /// </summary>
        public IList<string> SubscriberIds { get; private set; }

        /// <summary>
        /// 发送服务类型(用于限定信息模板)
        /// </summary>
        public SendingServiceType SendingServiceType { get; private set; }

        public Theme(string name, string loginName, SubscriberType subscriberType, bool enabled, IList<string> templateIds, IList<string> subscriberIds, SendingServiceType sendingServiceType)
        {
            Name = name;
            LoginName = loginName;
            SubscriberType = subscriberType;
            Enabled = enabled;
            TemplateIds = templateIds;
            SubscriberIds = subscriberIds;
            SendingServiceType = sendingServiceType;
        }
    }
}
