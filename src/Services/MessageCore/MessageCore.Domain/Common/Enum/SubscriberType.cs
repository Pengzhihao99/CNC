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
    /// 订阅者类型
    /// </summary>
    public class SubscriberType : Enumeration
    {
        public SubscriberType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 客户
        /// </summary>
        [Description("Client")]
        public static SubscriberType Client = new SubscriberType(1, "Client");

        /// <summary>
        /// 员工
        /// </summary>
        [Description("Employee")]
        public static SubscriberType Employee = new SubscriberType(2, "Employee");
    }
}
