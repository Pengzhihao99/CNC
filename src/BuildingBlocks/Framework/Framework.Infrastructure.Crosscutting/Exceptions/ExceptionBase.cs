using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Crosscutting.Exceptions
{
    /// <summary>
    /// 定义的一个异常基类
    /// </summary>
    public abstract class ExceptionBase : Exception
    {
        /// <summary>
        /// </summary>
        protected ExceptionBase()
        {
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode)
        {
            Init(errorCode, null);
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode, Exception innerException)
            : base(null, innerException)
        {
            Init(errorCode, null);
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode, string message)
        {
            Init(errorCode, message);
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode, string message, Exception innerException)
            : base(null, innerException)
        {
            Init(errorCode, message);
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode, string messageFormat, params object[] messageArgs)
        {
            Init(errorCode, String.Format(messageFormat, messageArgs));
        }

        /// <summary>
        /// </summary>
        protected ExceptionBase(string errorCode, Exception innerException, string messageFormat, params object[] messageArgs)
            : base(null, innerException)
        {
            Init(errorCode, String.Format(messageFormat, messageArgs));
        }

        /// <summary>
        /// </summary>
        void Init(string errorCode, string message)
        {
            ErrorCode = errorCode;
            CustomMessage = message;
        }

        /// <summary>
        /// 系统自定义的错误代码
        /// </summary>
        public string ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 系统自定义的错误信息
        /// </summary>
        public string CustomMessage { get; private set; }

        /// <summary>
        /// 重写基类属性Message
        /// </summary>
        public override string Message => $"[ErrorCode: {ErrorCode}] {CustomMessage}";
    }
}
