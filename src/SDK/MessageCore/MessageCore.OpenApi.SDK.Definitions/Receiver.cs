namespace MessageCore.OpenApi.SDK.Definitions
{
    public class Receiver
    {
        /// <summary>
        /// 订阅者名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 企业微信账号
        /// </summary>
        public string EnterpriseWeChat { get; set; }
    }
}
