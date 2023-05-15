using Framework.Application.Core.Queries;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries
{
    public interface IIssuerQueries
    {
        Task<PagingFindResultWrapper<IssuerVM>> GetIssuerByPageAsync(IssuerQuery query);

        Task<PagingFindResultWrapper<IssuerVM>> GetIssuerByNameOrStatusAsync(string SenderName, string status);
    }
}
