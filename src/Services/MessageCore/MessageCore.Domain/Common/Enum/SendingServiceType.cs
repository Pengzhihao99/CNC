using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    /// <summary>
    /// 发送服务类型
    /// </summary>
    public class SendingServiceType : Enumeration
    {
        public SendingServiceType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 手机
        /// </summary>
        [Description("Phone")]
        public static SendingServiceType Phone = new(1, "Phone");

        /// <summary>
        /// 邮件
        /// </summary>
        [Description("Email")]
        public static SendingServiceType Email = new(2, "Email");

        /// <summary>
        /// 企业微信
        /// </summary>
        [Description("EnterpriseWeChat")]
        public static SendingServiceType EnterpriseWeChat = new(3, "EnterpriseWeChat");


    }
}
