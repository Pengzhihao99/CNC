using MediatR;
using Framework.Application.Core.Commands;

namespace MessageCore.Application.Admin.Commands.SubscriberCommand
{
    public class CreateSubscriberCommand : IRequest, ICommand
    {
        /// <summary>
        /// 订阅者代码
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

        /// <summary>
        /// 订阅者类型
        /// </summary>
        public int SubscriberType { get; set; }

        /// <summary>
        /// 组，从属于，客户代码
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
