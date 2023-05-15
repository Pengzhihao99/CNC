using Framework.Domain.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Models
{
    public abstract class IndexConfiguration
    {
        public string IndexName { get; set; }

        /// <summary>
        /// 实体的类型
        /// </summary>
        public Type EntityType { get; set; }
    }

    public class IndexConfiguration<TAggregateRoot> : IndexConfiguration
       where TAggregateRoot : IAggregateRoot
    {
        public IndexKeysDefinition<TAggregateRoot> IndexKeysDefinition { get; set; }

        public CreateIndexOptions CreateIndexOptions { get; set; }

        public IndexConfiguration()
        {
            base.EntityType = typeof(TAggregateRoot);
        }
    }
}
