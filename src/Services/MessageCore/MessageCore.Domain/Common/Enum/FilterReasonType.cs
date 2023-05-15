using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    public class FilterReasonType : Enumeration
    {
        public FilterReasonType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 黑名单
        /// </summary>
        [Description("黑名单")]
        public static FilterReasonType Blacklist = new FilterReasonType(1, "Blacklist");

        /// <summary>
        /// 订阅者不可用
        /// </summary>
        [Description("订阅者不可用")]
        public static FilterReasonType SubscriberUnable = new FilterReasonType(2, "SubscriberUnable");

        /// <summary>
        /// 订阅者信息为空
        /// </summary>
        [Description("订阅者信息为空")]
        public static FilterReasonType SubscriberReceiveWayNull = new FilterReasonType(3, "SubscriberReceiveWayNull");

        /// <summary>
        /// 订阅者类型错误
        /// </summary>
        [Description("订阅者类型错误")]
        public static FilterReasonType SubscriberTypeError = new FilterReasonType(4, "SubscriberTypeError");

    }
}
