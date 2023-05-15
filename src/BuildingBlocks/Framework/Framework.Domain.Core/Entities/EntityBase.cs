using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Entities
{

    /// <summary>
    /// 实体类，具有一个Id属性作为其唯一标识。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Serializable]
    public abstract class EntityBase<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected EntityBase()
        {
            //this.Id = default(TKey);
            //this.RowVersion = Guid.NewGuid().ToString("N");
        }

        protected EntityBase(TKey id)
            : this()
        {
            Id = id;
        }

        public virtual TKey Id { get; set; }
        //public virtual string RowVersion { get; protected set; }

        //public bool IsTransient()
        //{
        //    return this.Id == default(TKey);
        //}
    }
}
