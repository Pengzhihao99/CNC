using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.ThemeAggregate.Specifications
{
    public class MatchThemeByIdSpecification<T> : Specification<T>
        where T : Theme
    {
        public string Id { get; private set; }
        public MatchThemeByIdSpecification(string id)
        {
            Id = id;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.Id.Equals(Id);
        }
    }
}