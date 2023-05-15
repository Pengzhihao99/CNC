﻿using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.OrderFilterAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlyOrderFilterRepository : ReadOnlyMongoDBRepository<OrderFilter, string>, IReadOnlyOrderFilterRepository
    {
        public ReadOnlyOrderFilterRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
