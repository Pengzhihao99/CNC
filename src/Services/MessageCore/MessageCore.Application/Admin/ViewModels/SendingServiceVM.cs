using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.ViewModels
{
    public class SendingServiceVM
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 发送服务类型
        /// </summary>
        public SendingServiceType SendingServiceType { get; set; }

        /// <summary>
        /// HOST
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
