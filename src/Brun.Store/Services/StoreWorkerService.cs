using Brun.Commons;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Models;
using Brun.Observers;
using Brun.Services;
using Brun.Store.Entities;
using Brun.Workers;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Services
{
    //TODO 从数据库加载数据 初始化内存worker
    public class StoreWorkerService : WorkerService, IWorkerService
    {
        readonly SqlSugarClient db;
        public StoreWorkerService(SqlSugarClient sqlSugarClient, WorkerServer workerServer) : base(workerServer)
        {
            this.db = sqlSugarClient;
        }
        /// <summary>
        /// //TODO 已存在数据库，启动时初始化内存对象+单元测试
        /// </summary>
        /// <returns></returns>
        public async Task<BrunResultState> ReStartInit()
        {
            var dbHasWorker = await db.Queryable<WorkerEntity>().AnyAsync();
            if (dbHasWorker)
            {
                List<WorkerEntity> workers = await db.Queryable<WorkerEntity>().ToListAsync();
                foreach (var item in workers)
                {
                    Type workerType = BrunTool.GetTypeByWorkerName(item.Type);
                    if (workerType == null)
                    {
                        throw new BrunException(BrunErrorCode.ObjectIsNull, $"the worker type in db '{item.Type}' is not supported");
                    }
                }
            }
            throw new BrunException(BrunErrorCode.ObjectIsNull, $"the worker type in db '' is not supported");
        }
        public override async Task<IWorker> AddWorker(WorkerConfig config, Type workerType, bool autoStart = true, bool addRunDetailObserver = true)
        {
            if (addRunDetailObserver)
            {
                config.AddWorkerObserver(new List<WorkerObserver>()
                {
                    //TODO 换成Store Observer
                    new WorkerStartRunInMemoryObserver(),
                    new WorkerExceptRunInMemoryObserver(),
                    new WorkerEndRunInMemoryObserver(),
                });
            }
            try
            {
                db.BeginTran();
                bool hasWorker = await db.Queryable<WorkerEntity>().AnyAsync(m => m.Id == config.Key);
                if (hasWorker)
                    throw new BrunException(BrunErrorCode.AllreadyKey, $"store worker allready has key '{config.Key}'");
                WorkerEntity entity = new WorkerEntity();
                entity.Id = config.Key;
                entity.Name = config.Name;
                entity.Type = workerType.Name;
                entity.State = WorkerState.Started;
                int dbr = await db.Insertable(entity).ExecuteCommandAsync();
                if (dbr == 0)
                    throw new BrunException(BrunErrorCode.StoreServiceError, "store add worker return 0");
                //调用基类内存操作,autoStart=false,避免重复Start
                IWorker baseResult = await base.AddWorker(config, workerType, false, false);
                if (baseResult == null)
                    throw new BrunException(BrunErrorCode.MemoryServiceError, "memory add worker is not success");
                else
                    db.CommitTran();
                if (autoStart)
                    this.Start(baseResult.Key);
                return baseResult;
            }
            catch (Exception)
            {
                db.RollbackTran();
                throw;
            }
            finally
            {
                db.Dispose();
            }
        }

        public override async Task<TWorker> AddWorker<TWorker>(WorkerConfig config, bool autoStart = true)
        {
            return (TWorker)(await AddWorker(config, typeof(TWorker), autoStart));
        }
        public override async Task<IOnceWorker> AddOnceWorker(WorkerConfig workerConfig, bool autoStart = true)
        {
            return await AddWorker<OnceWorker>(workerConfig, autoStart);
        }
        public override async Task<ITimeWorker> AddTimeWorker(WorkerConfig workerConfig, bool autoStart = true)
        {
            return await AddWorker<TimeWorker>(workerConfig, autoStart);
        }
        public override async Task<IQueueWorker> AddQueueWorker(WorkerConfig config, bool autoStart = true)
        {
            return await AddWorker<QueueWorker>(config, autoStart);
        }
        public override async Task<IPlanWorker> AddPlanWorker(WorkerConfig config, bool autoStart = true)
        {
            return await AddWorker<PlanWorker>(config, autoStart);
        }
        public override async Task<(IEnumerable<WorkerInfo>, int)> GetWorkerInfos(int current, int pageSize)
        {
            int total = await db.Queryable<WorkerEntity>().CountAsync();
            var data = await db.Queryable<WorkerEntity>().OrderBy(m => m.Name).Skip(pageSize * (current - 1)).Take(pageSize).Select<WorkerInfo>(m => new WorkerInfo()
            {
                Key = m.Id,
                Name = m.Name,
                TypeName = m.Type,
                State = m.State
            }).ToListAsync();
            return (data, total);
        }

        public override void Start(string key)
        {
            int dbr = db.Updateable<WorkerEntity>().Where(m => m.Id == key).UpdateColumns(m => m.State == WorkerState.Started).ExecuteCommand();
            if (dbr > 0)
            {
                base.Start(key);
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"worker start error,can not find key:'{key}'");
            }
        }

        public override void StartAll()
        {
            db.Updateable<WorkerEntity>().UpdateColumns(m => m.State == WorkerState.Started).ExecuteCommand();
            base.StartAll();
        }

        public override void Stop(string key)
        {
            int dbr = db.Updateable<WorkerEntity>().Where(m => m.Id == key).UpdateColumns(m => m.State == WorkerState.Stoped).ExecuteCommand();
            if (dbr > 0)
            {
                base.Stop(key);
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"worker stop error,can not find key:'{key}'");
            }
        }

        public override void StopAll()
        {
            db.Updateable<WorkerEntity>().UpdateColumns(m => m.State == WorkerState.Stoped).ExecuteCommand();
            base.StopAll();
        }

        public override void StopByName(string name)
        {
            throw new NotImplementedException();
        }

        public override void StartByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
