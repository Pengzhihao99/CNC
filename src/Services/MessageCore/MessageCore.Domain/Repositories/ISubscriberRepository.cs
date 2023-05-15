using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Repositories
{
    public interface ISubscriberRepository : IRepository<Subscriber, string>
    {
    }
}
