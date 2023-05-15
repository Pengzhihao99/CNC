using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.IssuerAggregate
{
    /// <summary>
    /// 发送者管理
    /// </summary>
    public class Issuer : AggregateRoot
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Issuer(string name, string token, bool enabled, string remark) 
        {
            Name = name;
            Token = token;
            Enabled = enabled;
            Remark = remark;
        }
    }
}
