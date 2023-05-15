using MailKit.Net.Smtp;
using MessageCore.Application.DataTransferModels;
using MessageCore.Application.SendingServices.Base;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using MessageCore.Domain.AggregatesModels.SendingAttachmentAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.SendingServices
{
    public class MailKitEmailService : IEmailService
    {
        public async Task SendAsync(From from, To to, string subject, string content, IList<SendingAttachment> attachments, Account account)
        {
            //附件
            var attachmentDatas = attachments;

            var message = new MimeMessage();
            //发件人
            message.From.Add(new MailboxAddress(from.Address, from.Address));
            //收件人
            message.To.Add(new MailboxAddress(to.Address, to.Address));
            //主题
            message.Subject = subject;
            //内容
            var htmlBody = content;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            var multipart = new Multipart("mixed")
            {
                builder.ToMessageBody()
            };
            //处理附件
            if (attachmentDatas != null && attachmentDatas.Count() > 0)
            {
                foreach (var attachmentData in attachmentDatas)
                {
                    //https://github.com/jstedfast/MimeKit/blob/master/MimeKit/MimeTypes.cs
                    var attachment = new MimePart(attachmentData.Type.MimeType)
                    {
                        Content = new MimeContent(new MemoryStream(attachmentData.Data), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Default,
                        FileName = attachmentData.Name,
                        IsAttachment = true,
                    };

                    multipart.Add(attachment);
                }
            }
            message.Body = multipart;

            //发送
            using var client = new SmtpClient();
            int port = int.Parse(account.Host.Split(':')[1]);
            var host = account.Host.Split(':')[0];

            client.Connect(host, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(account.UserName, account.Password);
            await client.SendAsync(message);
            client.Disconnect(true);
        }
    }
}
