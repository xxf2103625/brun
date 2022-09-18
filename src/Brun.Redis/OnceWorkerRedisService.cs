//using Brun.BaskRuns;
//using Brun.Models;
//using Brun.Options;
//using Brun.Services;
//using Brun.Workers;
//using StackExchange.Redis;
//using System.Text.Json;

//namespace Brun.Redis
//{
//    /// <summary>
//    /// 基于Redis的Brun持久化管理
//    /// </summary>
//    public class OnceWorkerRedisService : IOnceBrunService
//    {
//        //private WorkerServer _workerServer;
//        //ConnectionMultiplexer _conn;
//        IDatabase _db;
//        public OnceWorkerRedisService( ConnectionMultiplexer connection)
//        {
//            _db = connection.GetDatabase();
//        }
//        public async Task<BrunResultState> AddOnceBrun(WorkerConfigModel model)
//        {
//            throw new NotImplementedException();
//            //if (model.Key == null)
//            //{
//            //    model.Key = Guid.NewGuid().ToString();
//            //}
//            //if (model.Name == null)
//            //{
//            //    model.Name = nameof(OnceWorker);
//            //}
//            //WorkerInfo entity = new WorkerInfo()
//            //{
//            //    Key = model.Key,
//            //    Name = model.Name
//            //};
//            //string onceWorkerKey = CacheKeys.OnceWorkKey;
//            //RedisValue[]? list = await _db.SetMembersAsync(onceWorkerKey);
//            //if (list != null && list.Any(m => JsonSerializer.Deserialize<WorkerInfo>(m)?.Key == model.Key))
//            //{
//            //    //已有
//            //    return BrunResultState.IdBeUsed;
//            //}
//            //_baseService.AddWorker(model);
//            //if (await _db.SetAddAsync(onceWorkerKey, JsonSerializer.Serialize(entity)))
//            //{
//            //    return BrunResultState.Success;
//            //}
//            //else
//            //{
//            //    throw new Exception("未知异常,redis插入数据失败");
//            //}

//        }

//        public BrunResultState AddOnceBrun(IOnceWorker onceWorker, Type brunType,OnceBackRunOption option)
//        {
//            throw new NotImplementedException();
//        }

//        public  IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns()
//        {
//            throw new NotImplementedException();
//            //var list = await _db.SetMembersAsync(CacheKeys.OnceWorkKey);
//            //List<WorkerInfo> workers = new List<WorkerInfo>();
//            //foreach (var item in list)
//            //{
//            //    var worker = JsonSerializer.Deserialize<WorkerInfo>(item);
//            //    if (worker != null)
//            //        workers.Add(worker);
//            //}
//            //return workers;
//        }

//        public IEnumerable<ValueLabel> GetOnceWorkersInfo()
//        {
//            throw new NotImplementedException();
//        }

//        public IOnceWorker GetWorker(string key)
//        {
//            throw new NotImplementedException();
//        }

//        Task<OnceBackRun> IOnceBrunService.AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option)
//        {
//            throw new NotImplementedException();
//        }

//        IAsyncEnumerable<KeyValuePair<string, IBackRun>> IOnceBrunService.GetOnceBruns()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}