
using MessageCore.Domain.Common.Enum;

namespace MessageCore.Application.Admin.ViewModels;

public class SubscriberVM
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }

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

    /// <summary>
    /// 订阅者类型
    /// </summary>
    public SubscriberType SubscriberType { get; set; }

    /// <summary>
    /// 组，从属于，客户代码
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}