using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.BlockingAggregate.Specifications
{

    public class MatchBlockingByTemplateNameSpecification<T> : Specification<T>
      where T : Blocking
    {
        public string TemplateName{ get; private set; }
        public MatchBlockingByTemplateNameSpecification(string templateName)
        {
            TemplateName = templateName;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.TemplateName == TemplateName;
        }
    }
}
