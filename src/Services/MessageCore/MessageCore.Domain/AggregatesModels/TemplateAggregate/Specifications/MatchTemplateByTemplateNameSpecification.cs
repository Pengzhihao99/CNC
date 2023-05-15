using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.TemplateAggregate.Specifications
{
    public class MatchTemplateByTemplateNameSpecification<T> : Specification<T>
    where T : Template
    {
        public string TemplateName { get; private set; }
        public MatchTemplateByTemplateNameSpecification(string templateName)
        {
            TemplateName = templateName;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.TemplateName.Contains(TemplateName);
        }
    }
}
