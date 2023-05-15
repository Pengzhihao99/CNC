using MessageCore.Application.OpenApi.DataTransferModels;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services.Base
{

    public interface IAttachmentService
    {
        public Task<string> SaveAsync(AttachmentDto attachmentDto);

        public Task<SendingAttachment> GetAsync(string id);
    }
}
