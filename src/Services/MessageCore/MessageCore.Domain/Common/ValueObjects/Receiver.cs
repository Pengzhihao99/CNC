
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Entities;

namespace MessageCore.Domain.Common.ValueObjects
{

    /// <summary>
    /// 收件人
    /// </summary>
    public class Receiver : ValueObject
    {
        /// <summary>
        /// 订阅者名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 企业微信账号
        /// </summary>
        public string EnterpriseWeChat { get; set; }

        /// <summary>
        /// 订阅者类型(这边暂时不考虑这个类型过滤)
        /// </summary>
        //public SubscriberType SubscriberType { get;  set; }

        protected Receiver()
        {

        }

        public Receiver(string name, string email, string phone, string enterpriseWeChat)
        {
            Name = name;
            Email = email;
            Phone = phone;
            EnterpriseWeChat = enterpriseWeChat;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Email;
            yield return Phone;
            yield return EnterpriseWeChat;

        }
    }
}
