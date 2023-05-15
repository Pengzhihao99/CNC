using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Models
{
    /// <summary>
    /// Mongo 实例的基本信息
    /// </summary>
    public class MongoInstanceInfo
    {
        /// <summary>
        /// </summary> 
        public MongoInstanceInfo(BsonDocument sourceData)
        {
            //Check.Argument.IsNotNull(sourceData, nameof(sourceData));

            SourceData = sourceData;

            // 示例： 连接Mongos 实例的 返回结果
            // { "isWritablePrimary" : true, "msg" : "isdbgrid", "maxBsonObjectSize" : 16777216, "maxMessageSizeBytes" : 48000000, "maxWriteBatchSize" : 100000, "localTime" : ISODate("2021-06-22T09:09:12.945Z"),
            //   "logicalSessionTimeoutMinutes" : 30, "connectionId" : 61258, "maxWireVersion" : 9, "minWireVersion" : 0, "topologyVersion" : { "processId" : ObjectId("60a1df5a6f48712ec8f807d1"), "counter" : NumberLong(0) },
            //    "ok" : 1.0, "operationTime" : Timestamp(1624352952, 2),
            //   "$clusterTime" : { "clusterTime" : Timestamp(1624352952, 2), "signature" : { "hash" : new BinData(0, "Vn4Al5uRaQ+xrP4lgmLVFbqVJfA="), "keyId" : NumberLong("6910028657532600342") } } }

            // 示例： 连接副本集实例的 返回结果
            // { "topologyVersion" : { "processId" : ObjectId("603363cad72d33427a27f199"), "counter" : NumberLong(10) }, "hosts" : ["192.168.1.96:27017", "192.168.1.96:27018"], "setName" :
            //   "rs0", "setVersion" : 25, "isWritablePrimary" : true, "secondary" : false, "primary" : "192.168.1.96:27017", "me" : "192.168.1.96:27017", "electionId" : ObjectId("7fffffff000000000000003e"),
            //   "lastWrite" : { "opTime" : { "ts" : Timestamp(1624353074, 9), "t" : NumberLong(62) }, "lastWriteDate" : ISODate("2021-06-22T09:11:14Z"),
            // "majorityOpTime" : { "ts" : Timestamp(1624353074, 9), "t" : NumberLong(62) }, "majorityWriteDate" : ISODate("2021-06-22T09:11:14Z") },
            // "maxBsonObjectSize" : 16777216, "maxMessageSizeBytes" : 48000000, "maxWriteBatchSize" : 100000, "localTime" : ISODate("2021-06-22T09:11:15.39Z"),
            // "logicalSessionTimeoutMinutes" : 30, "connectionId" : 1721210, "minWireVersion" : 0, "maxWireVersion" : 9, "readOnly" : false, "ok" : 1.0,
            // "$clusterTime" : { "clusterTime" : Timestamp(1624353074, 9), "signature" : { "hash" : new BinData(0, "bLaxQPwC4oIyVTiWyugzSwqNILA="),
            // "keyId" : NumberLong("6939416477334241288") } }, "operationTime" : Timestamp(1624353074, 9) }

            // 示例： 连接本地单实例，无分片，无副本集
            // { "ismaster" : true, "maxBsonObjectSize" : 16777216, "maxMessageSizeBytes" : 48000000, "maxWriteBatchSize" : 100000, "localTime" : ISODate("2021-06-22T09:50:49.264Z"),
            //    "logicalSessionTimeoutMinutes" : 30, "minWireVersion" : 0, "maxWireVersion" : 7, "readOnly" : false, "ok" : 1.0 }

            //判断是否包含msg元素字段以及字段值是否为isdbgrid， 是则表示链接的是
            if (sourceData.Contains("msg") && "isdbgrid".Equals(sourceData.GetElement("msg").Value.AsString))
            {
                IsShardedInstance = true;
            }
            //判断是否是副本集
            else if (sourceData.Contains("hosts") && sourceData.Contains("setName"))
            {
                IsReplicaSetInstance = true;
            }
            else
            {
                //直连数据库
            }
        }

        /// <summary>
        /// 从数据库中获取的Mongo 实例的原始信息。通过调用 db.isMaster()  / db.runCommand( { hello: 1 } ) 获取
        /// </summary>
        public BsonDocument SourceData { get; set; }

        /// <summary>
        /// 连接的 是否是 分片，也就是是否是连接到Mongos服务
        /// </summary>
        public bool IsShardedInstance { get; protected set; }

        /// <summary>
        /// 是否是 副本集 </summary>
        public bool IsReplicaSetInstance { get; protected set; }

    }
}
