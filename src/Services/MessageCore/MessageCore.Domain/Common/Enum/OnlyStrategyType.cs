using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    public class OnlyStrategyType : Enumeration
    {
        public OnlyStrategyType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 24小时
        /// </summary>
        [Description("24小时")]
        public static OnlyStrategyType TwentyFourHour = new OnlyStrategyType(1, "TwentyFourHour");

        /// <summary>
        /// 12小时
        /// </summary>
        [Description("12小时")]
        public static OnlyStrategyType TwelveHour = new OnlyStrategyType(2, "TwelveHour");

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        public static OnlyStrategyType None = new OnlyStrategyType(3, "None");
    }
}
