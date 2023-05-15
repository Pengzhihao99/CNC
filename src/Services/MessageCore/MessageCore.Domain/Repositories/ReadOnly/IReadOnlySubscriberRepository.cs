using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Repositories.ReadOnly
{
    public interface IReadOnlySubscriberRepository : IReadOnlyRepository<Subscriber, string>
    {
    }
}
