using Framework.Infrastructure.Crosscutting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Builder
{
    /// <summary>
    /// Mongo 序列化配置信息 构建器
    /// </summary>
    public class MongoSerializationConfigBuilder
    {
        private readonly List<IConvention> _conventions;

        /// <summary>
        /// </summary> 
        public MongoSerializationConfigBuilder()
        {
            _conventions = new List<IConvention>();
        }

        public virtual MongoSerializationConfigBuilder RegisterConvention(IConvention convention)
        {
            Check.Argument.IsNotNull(convention, nameof(convention));

            _conventions.Add(convention);

            return this;
        }

        public virtual MongoSerializationConfigBuilder RegisterSerializer(Type type, IBsonSerializer bsonSerializer)
        {
            Check.Argument.IsNotNull(type, nameof(type));
            Check.Argument.IsNotNull(bsonSerializer, nameof(bsonSerializer));

            BsonSerializer.RegisterSerializer(type, bsonSerializer);
            return this;
        }

        public virtual void BuildAndConfigure()
        {
            if (_conventions.Any())
            {
                var conventionPack = new ConventionPack();

                _conventions.ForEach(x => conventionPack.Add(x));

                ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
            }
        }
    }
}
