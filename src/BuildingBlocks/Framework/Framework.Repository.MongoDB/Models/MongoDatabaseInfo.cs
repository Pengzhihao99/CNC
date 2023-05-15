using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Framework.Repository.MongoDB.Models
{
    public class MongoDatabaseInfo
    {
        public MongoDatabaseInfo(string databaseName)
        {
            DatabaseName = databaseName;

            CollectionNames = new HashSet<string>(50);
            CollectionShardKeys = new Dictionary<string, IList<BsonElement>>();
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// 数据库是否已经启动分片
        /// </summary>
        public bool IsPartitioned { get; set; }

        /// <summary>
        /// 数据库中 MongoDB 集合名称列表
        /// </summary>
        public HashSet<string> CollectionNames { get; set; }

        /// <summary>
        /// 分片的集合名称以及对应的分片键（BsonElement表示（就是一个键值对， 如 "_id" : "hashed", "CreateOperation.Time" : 1.0 等等））
        /// </summary>
        public Dictionary<string, IList<BsonElement>> CollectionShardKeys { get; set; }

        /// <summary>
        /// 是否无权限访问config数据库中的名为databases的集合
        /// </summary>
        public bool IsNotAuthorizedAccseeDatabaseCollectionInConfigs { get; set; }

        /// <summary>
        /// 是否无权限访问config数据库中的名为collections的集合
        /// </summary>
        public bool IsNotAuthorizedAccseeCollectionsCollectionInConfigs { get; set; }

        /// <summary>
        /// 是否无权限对数据库执行 enableSharding
        /// </summary>
        public bool IsNotAuthorizedEnableSharding { get; set; }

        /// <summary>
        /// 是否无权限执行 shardCollection
        /// </summary>
        public bool IsNotAuthorizedShardCollection { get; set; }
    }
}
