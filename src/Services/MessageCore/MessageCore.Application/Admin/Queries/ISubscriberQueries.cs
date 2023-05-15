using Framework.Application.Core.Queries;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;

namespace MessageCore.Application.Admin.Queries;

public interface ISubscriberQueries
{
    /// <summary>
    /// 分页获取订阅者信息
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagingFindResultWrapper<SubscriberVM>> GetSubscribersByPageAsync(SubscriberQuery query);
}