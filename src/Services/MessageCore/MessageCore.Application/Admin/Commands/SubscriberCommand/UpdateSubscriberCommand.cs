using Framework.Application.Core.Commands;
using MediatR;

namespace MessageCore.Application.Admin.Commands.SubscriberCommand;

public class UpdateSubscriberCommand : IRequest, ICommand
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }

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
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}