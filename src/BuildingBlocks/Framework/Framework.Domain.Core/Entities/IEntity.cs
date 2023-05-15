using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Entities
{
    /// <summary>
    /// 定义一个实体，具有一个Id属性作为其唯一标识。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>: IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Id，唯一标识 (Unique identifier for this entity.)
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// 定义一个实体
    /// </summary>
    public interface IEntity
    {

    }

}
