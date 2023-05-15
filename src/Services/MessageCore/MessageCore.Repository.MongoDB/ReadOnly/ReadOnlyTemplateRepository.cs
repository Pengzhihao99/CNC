using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{

    public class ReadOnlyTemplateRepository : ReadOnlyMongoDBRepository<Template, string>, IReadOnlyTemplateRepository
    {
        public ReadOnlyTemplateRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
