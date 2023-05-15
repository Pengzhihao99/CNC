using Framework.Infrastructure.Crosscutting.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Exceptions
{

    public class MessageCoreInternalException : ExceptionBase
    {
        public MessageCoreInternalException(string errorCode, string message)
           : base(errorCode, message)
        {
            Type = GetType().Name;
        }

        public string Type { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public override string Message => $"{CustomMessage} ({ErrorCode}) ";
    }
}
