using Framework.Domain.Core.Specification;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingAttachmentAggregate.Specifications
{
    public class MatchSendingAttachmentByIdsSpecification<T> : Specification<T>
    where T : SendingAttachment
    {
        public IList<string> Ids { get; private set; }
        public MatchSendingAttachmentByIdsSpecification(IList<string> ids)
        {
            Ids = ids;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            if (Ids == null || Ids.Count < 1)
            {
                return id => false;
            }
            else
            {
                return item => Ids.Contains(item.Id);
            }
        }
    }
}
