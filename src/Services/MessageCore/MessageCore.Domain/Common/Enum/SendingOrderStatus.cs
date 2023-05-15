using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    public class SendingOrderStatus : Enumeration
    {
        public SendingOrderStatus(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 等待发送
        /// </summary>
        [Description("Ready")]
        public static SendingOrderStatus Ready = new SendingOrderStatus(1, "Ready");

        /// <summary>
        /// 发送中
        /// </summary>
        [Description("Sending")]
        public static SendingOrderStatus Sending = new SendingOrderStatus(2, "Sending");

        /// <summary>
        /// 成功
        /// </summary>
        [Description("Success")]
        public static SendingOrderStatus Success = new SendingOrderStatus(3, "Success");

        /// <summary>
        /// 失败
        /// </summary>
        [Description("Fail")]
        public static SendingOrderStatus Fail = new SendingOrderStatus(4, "Fail");
    }
}
