using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.UOW
{
    public class MongoDBUnitOfWork : DisposableObject, IUnitOfWork
    {
        private readonly IMongoDBContext _mongoDbContext;

        /// <summary>
        /// </summary> 
        public MongoDBUnitOfWork(IMongoDBContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public bool IsCommitted { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public bool IsRollback { get; protected set; }

        public void Begin()
        {
            //开启事务
            _mongoDbContext.StartTransaction();
        }

        public async Task CommitAsync()
        {
            //如果回滚了，不能提交，正常情况下需要重新建立一个新的UnitOfWork
            if (IsRollback)
            {
                return;
            }

            if (!IsCommitted)
            {
                if (_mongoDbContext.Session != null && _mongoDbContext.Session.IsInTransaction)
                {
                    CancellationToken cancellationToken = default;
                    await _mongoDbContext.Session.CommitTransactionAsync(cancellationToken);
                }

                IsCommitted = true;
                IsRollback = false;
                //this.OnCommitted();
            }
            else
            {
                throw new Exception("Commit is called before!");
            }
        }

        public async Task RollbackAsync()
        {
            //如果回滚了，不重复回滚
            if (IsRollback)
            {
                return;
            }

            //如果在事务中，回滚。
            if (_mongoDbContext.Session != null && _mongoDbContext.Session.IsInTransaction)
            {
                CancellationToken cancellationToken = default;
                await _mongoDbContext.Session.AbortTransactionAsync(cancellationToken);
            }

            IsCommitted = false;
            IsRollback = true;

            //this.OnRollback();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                //如果没有 提交, 也没有回滚。
                //理论上在释放的时候要回滚。
                if (!IsCommitted && !IsRollback)
                {
                    RollbackAsync().GetAwaiter().GetResult();
                }

                if (disposing)
                {
                    _mongoDbContext?.Session?.Dispose();

                    IsDisposed = true;
                }

                //this.OnDisposed();
            }
        }
    }
}
