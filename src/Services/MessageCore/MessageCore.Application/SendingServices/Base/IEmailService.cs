using MessageCore.Application.DataTransferModels;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.SendingServices.Base
{
    public interface IEmailService
    {
        public Task SendAsync(From from, To to, string subject, string content, IList<SendingAttachment> attachments, Account account);
    }
}
