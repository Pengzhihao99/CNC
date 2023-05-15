using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Models
{
    public class MongoDBConnectionOptions
    {
        /// <summary>
        /// MongoDB连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 指定的MongoDB数据库
        /// </summary>
        public string Database { get; set; }
    }
}
