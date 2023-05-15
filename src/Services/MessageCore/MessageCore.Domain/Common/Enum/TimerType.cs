using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    public class TimerType : Enumeration
    {
        public TimerType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 立即
        /// </summary>
        [Description("立即")]
        public static TimerType Immediately = new(1, "Immediately");

        /// <summary>
        /// 间隔1小时
        /// </summary>
        [Description("间隔1小时")]
        public static TimerType Employee = new(2, "EveryOneHour");
    }
}
