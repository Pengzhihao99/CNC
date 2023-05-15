using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Exceptions
{
    public class ExceptionType
    {
        public static string MessageCoreExternalException = typeof(MessageCoreExternalException).Name;

        public static string MessageCoreInternalException = typeof(MessageCoreInternalException).Name;

        /// <summary>
        /// 未处理异常
        /// </summary>
        public static string MessageCoreUnhandledException = "MessageCoreUnhandledException";
        
    }
}
