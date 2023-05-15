using Framework.Domain.Core.Specification;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.TemplateAggregate.Specifications
{
    public class MatchTemplateByNameSpecification<T> : Specification<T>
    where T : Template
    {
        public string Name { get; private set; }
        public MatchTemplateByNameSpecification(string name)
        {
            Name = name;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.TemplateName.Contains(Name);
        }
    }
}
