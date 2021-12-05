using Brun.BaskRuns;
using Brun.Commons;
using Brun.Models;
using Brun.Options;
using Brun.Services;
using Brun.Store.Entities;
using Brun.Workers;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Services
{
    public class OnceWorkerStoreService : IOnceWorkerService
    {
        SqlSugarClient sqlSugarClient;
        public OnceWorkerStoreService( SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
        }

        public BrunResultState AddOnceBrun(OnceWorker onceWorker, Type brunType, BackRunOption option)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns()
        {
            //var list=await sqlSugarClient.Queryable<WorkerEntity>().Where(m=>m.Type==nameof(OnceWorker)).ToListAsync();
            //return list.Select(m => new WorkerInfo()
            //{
            //    Key = m.Id,
            //    Name= m.Name,
            //    TypeName=m.Type
            //}); 
            throw new NotImplementedException();
        }

        public IEnumerable<ValueLabel> GetOnceWorkersInfo()
        {
            throw new NotImplementedException();
        }
    }
}
