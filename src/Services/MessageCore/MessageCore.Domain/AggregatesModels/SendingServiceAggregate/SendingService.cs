using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingServiceAggregate

{
    /// <summary>
    /// 信息服务
    /// </summary>
    public class SendingService : AggregateRoot
    {
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

        public SendingService(string serviceName, SendingServiceType sendingServiceType, string host, string userName, string passWord, string appkey, string appSecret, string sender, bool enabled, string remark)
        {
            ServiceName = serviceName;
            SendingServiceType = sendingServiceType;
            Host = host;
            UserName = userName;
            PassWord = passWord;
            AppKey = appkey;
            AppSecret = appSecret;
            Sender = sender;
            Enabled = enabled;
            Remark = remark;
        }

        /// <summary>
        /// 修改服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="sendingServiceType"></param>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sender"></param>
        /// <param name="enabled"></param>
        /// <param name="remark"></param>
        public void ModifyInfo(string serviceName, SendingServiceType sendingServiceType, string host, string userName, string passWord, string appkey, string appSecret, string sender, bool enabled, string remark)
        {
            ServiceName = serviceName;
            SendingServiceType = sendingServiceType;
            Host = host;
            UserName = userName;
            PassWord = passWord;
            AppKey = appkey;
            AppSecret = appSecret;
            Sender = sender;
            Enabled = enabled;
            Remark = remark;
        }

    }
}
