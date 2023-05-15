using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.AttachmentAggregate
{
    /// <summary>
    /// 附件管理，这边分出来，可能后续不存在mongo下，需要用一个服务封一下
    /// </summary>
    public class SendingAttachment : AggregateRoot
    {
        /// <summary>
        /// 文件数据，16M限制
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 文件类型（考虑使用枚举）
        /// </summary>
        public AttachmentType Type { get; private set; }

        public SendingAttachment(byte[] data, string name, AttachmentType type)
        {
            Data = data;
            Name = name;
            Type = type;
        }
    }
}
