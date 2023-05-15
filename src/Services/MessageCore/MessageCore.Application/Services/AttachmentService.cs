using Framework.Domain.Core.Entities;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.IdGenerators;
using MessageCore.Application.OpenApi.DataTransferModels;
using MessageCore.Application.Services.Base;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services
{
    /// <summary>
    /// 附件存储服务
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private readonly ISendingAttachmentRepository _sendingAttachmentRepository;
        //private readonly IIdentityGenerator<string> _identityGenerator;
        public AttachmentService(ISendingAttachmentRepository sendingAttachmentRepository)
        {
            _sendingAttachmentRepository = sendingAttachmentRepository;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="attachmentDto"></param>
        /// <returns></returns>
        public async Task<string> SaveAsync(AttachmentDto attachmentDto)
        {
            Check.Argument.IsNotNull(attachmentDto, nameof(AttachmentDto));
            Check.Argument.IsNotNullOrEmpty(attachmentDto.Name, "AttachmentDto.Name");
            Check.Argument.IsNotNullOrEmpty(attachmentDto.Data, "AttachmentDto.Data");
            Check.Argument.IsNotNullOrEmpty(attachmentDto.AttachmentType, "AttachmentDto.AttachmentType");

            var attachmentType = Enumeration.FromDisplayName<AttachmentType>(attachmentDto.AttachmentType);

            var sendingAttachment = new Domain.AggregatesModels.AttachmentAggregate.SendingAttachment(Convert.FromBase64String(attachmentDto.Data), attachmentDto.Name, attachmentType);
            await _sendingAttachmentRepository.AddAsync(sendingAttachment);
            return sendingAttachment.Id;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MessageCoreInternalException"></exception>
        public async Task<SendingAttachment> GetAsync(string id)
        {
            var result = await _sendingAttachmentRepository.GetByKeyAsync(id);
            if (result == null)
            {
                throw new MessageCoreInternalException("000000", "Can not find Attachment by Id" + id);
            }
            return result;
        }
    }
}
