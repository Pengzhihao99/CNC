using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.ThemeAggregate.Specifications
{
    public class MatchThemeByTemplateIdSpecification<T> : Specification<T>
        where T : Theme
    {
        public string TemplateId { get; private set; }
        public MatchThemeByTemplateIdSpecification(string templateId)
        {
            TemplateId = templateId;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.TemplateIds.Any(item => item == TemplateId);
        }
    }
}