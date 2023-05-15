using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate
{
    /// <summary>
    /// 订阅者管理
    /// </summary>
    public class Subscriber : AggregateRoot
    {
        /// <summary>
        /// 订阅者名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// 企业微信账号
        /// </summary>
        public string EnterpriseWeChat{ get; private set; }

        /// <summary>
        /// 订阅者类型
        /// </summary>
        public SubscriberType SubscriberType { get; private set; }

        /// <summary>
        /// 组，从属于，客户代码
        /// </summary>
        public string Group { get; private set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        public Subscriber(string name, string email, string phone, string enterpriseWeChat, SubscriberType subscriberType, string group, bool enabled)
        {
            Name = name;
            Email = email;
            Phone = phone;
            EnterpriseWeChat = enterpriseWeChat;
            SubscriberType = subscriberType;
            Group = group;
            Enabled = enabled;
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="enterpriseWeChat"></param>
        /// <param name="subscriberType"></param>
        /// <param name="enabled"></param>
        public void ModifyInfo(string email, string phone, string enterpriseWeChat, SubscriberType subscriberType,
            bool enabled)
        {
            Email = email;
            Phone = phone;
            EnterpriseWeChat = enterpriseWeChat;
            SubscriberType = subscriberType;
            Enabled = enabled;
        }
    }
}
