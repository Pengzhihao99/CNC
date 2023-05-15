using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.IssuerAggregate.Specifications
{
    public class MatchIssuerByNameSpecification<T> : Specification<T>
    where T : Issuer
    {
        public string SenderName { get; private set; }
        public MatchIssuerByNameSpecification(string senderName)
        {
            SenderName = senderName;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.Name.Contains(SenderName);
        }
    }
}
