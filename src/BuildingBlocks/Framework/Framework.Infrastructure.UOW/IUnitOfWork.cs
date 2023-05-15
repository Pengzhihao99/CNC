using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.UOW
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值,该值表述了当前的Unit Of Work事务是否已被提交。
        /// </summary>
        bool IsCommitted { get; }

        /// <summary>
        ///  获得一个<see cref="System.Boolean"/>值,该值表述了当前的Unit Of Work事务是否被销毁了
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        ///  获得一个<see cref="System.Boolean"/>值,该值表述了当前的Unit Of Work事务是否被回滚了
        /// </summary>
        bool IsRollback { get; }

        /// <summary>
        /// Database transaction object, can be converted to a specific database transaction object or IDBTransaction when used
        /// </summary>
        //object DbTransaction { get; }

        /// <summary>
        /// 开始工作单元
        /// </summary>
        /// <returns></returns>
        void Begin();

        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        Task RollbackAsync();
    }
}
