//using Brun.BaskRuns;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace BrunWebTest
//{
//    public class TestScopeBackRun : ScopeBackRun
//    {
//        public override Task RunInScope(CancellationToken stoppingToken)
//        {
//            var service = GetRequiredService<ITestScopeService>();
//            string str = service.Todo();
//            Data["r"] = str;
//            return Task.CompletedTask;
//        }
//    }
//}
