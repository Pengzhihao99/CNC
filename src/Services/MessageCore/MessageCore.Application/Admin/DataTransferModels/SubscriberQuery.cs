using Framework.Application.Core.Queries;

namespace MessageCore.Application.Admin.DataTransferModels;

public class SubscriberQuery : AbstractPagingFindQuery
{
    /// <summary>
    /// 组，从属于，客户代码
    /// </summary>
    public string? Group { get; set; }
}