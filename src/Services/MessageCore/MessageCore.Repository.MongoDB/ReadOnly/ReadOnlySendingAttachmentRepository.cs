using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlySendingAttachmentRepository : ReadOnlyMongoDBRepository<SendingAttachment, string>, IReadOnlySendingAttachmentRepository
    {
        public ReadOnlySendingAttachmentRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
