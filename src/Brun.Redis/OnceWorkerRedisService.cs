using Brun.Models;
using Brun.Services;
using Brun.Workers;
using StackExchange.Redis;
using System.Text.Json;

namespace Brun.Redis
{
    /// <summary>
    /// 基于Redis的Brun持久化管理
    /// </summary>
    public class OnceWorkerRedisService : IOnceWorkerService
    {
        //private WorkerServer _workerServer;
        IBaseWorkerService<OnceWorker> _baseService;
        //ConnectionMultiplexer _conn;
        IDatabase _db;
        public OnceWorkerRedisService(IBaseWorkerService<OnceWorker> baseWorkerService, ConnectionMultiplexer connection)
        {
            _baseService = baseWorkerService;
            _db = connection.GetDatabase();
        }
        public async Task<BrunResultState> AddOnceWorker(WorkerConfigModel model)
        {
            if (model.Key == null)
            {
                model.Key = Guid.NewGuid().ToString();
            }
            if (model.Name == null)
            {
                model.Name = nameof(OnceWorker);
            }
            string onceWorkerKey = CacheKeys.OnceWorkKey;
            RedisValue[]? list = await _db.SetMembersAsync(onceWorkerKey);
            if (list != null && list.Any(m => JsonSerializer.Deserialize<WorkerConfigModel>(m)?.Key == model.Key))
            {
                //已有
                return BrunResultState.IdBeUsed;
            }
            _baseService.AddWorker(model);
            if (await _db.SetAddAsync(onceWorkerKey, JsonSerializer.Serialize(model)))
            {
                return BrunResultState.Success;
            }
            else
            {
                throw new Exception("未知异常,redis插入数据失败");
            }

        }

        public async Task<IEnumerable<OnceWorker>> GetOnceWorkers()
        {
            var list = await _db.SetMembersAsync(CacheKeys.OnceWorkKey);
            return list.Cast<OnceWorker>();
        }
    }
}