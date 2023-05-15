using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Crosscutting.IdGenerators
{
    /// <summary>
    /// 唯一标识生成器
    /// </summary>
    public interface IIdentityGenerator<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 生成一个Id
        /// </summary>
        T GenerateId();

        /// <summary>
        /// 生成多个Id
        /// </summary>
        /// <param name="count">指定生成的数量</param>
        IList<T> GenerateIds(int count);
    }
}
