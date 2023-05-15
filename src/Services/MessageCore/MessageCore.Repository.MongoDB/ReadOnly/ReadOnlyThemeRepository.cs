using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{

    public class ReadOnlyThemeRepository : ReadOnlyMongoDBRepository<Theme, string>, IReadOnlyThemeRepository
    {
        public ReadOnlyThemeRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
