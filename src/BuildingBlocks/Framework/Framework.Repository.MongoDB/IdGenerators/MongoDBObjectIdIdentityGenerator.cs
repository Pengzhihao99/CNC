using Framework.Infrastructure.Crosscutting.IdGenerators;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.IdGenerators
{
    public class MongoDBObjectIdIdentityGenerator : IIdentityGenerator<string>
    {
        public string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        /// <summary>
        /// 生成多个Id
        /// </summary>
        /// <param name="count">指定生成的数量</param>
        public IList<string> GenerateIds(int count)
        {
            if (count <= 0)
            {
                return new List<System.String>();
            }

            var idList = new List<System.String>(count);

            for (var i = 1; i <= count; i++)
            {
                idList.Add(GenerateId());
            }

            return idList;
        }
    }
}
