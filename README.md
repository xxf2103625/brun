
### Brun是netcore中轻量级后台任务组件，asp.net中简单易用
>定义了专门管理后台任务执行流程（触发条件）的Worker，和后台任务本身BackRun。Worker可以有多种类型，加上预定义的拦截器,可扩展性非常高，和第三方组件集成也容易。
- 适合http请求内触发的OnceWorker
- 适合消息队列触发的QueueWorker，目前是dotnet自带的Queue，扩展监听RabbitMQ第三方MQ也会非常容易（进行中）
- 适合计划时间执行的TimeWorker和PlanTimeWorker，一个用于简单的周期循环执行，一个用于复杂的计划事件内执行
- BackRun是类继承而不是直接用委托，更容易封装自己常用的后台任务，比如自己定义一个HttpBackRun，封装一些http的基础操作，所有请求任务可以直接继承HttpBackRun，发邮件任务、发系统异常日志任务等等都可以一次定义，所有项目共用

>目前仅支持内存运行，下个版本主要内容：
- 支持持久化到常用数据库或Redis缓存。
- 支持Server单独部署，Client端来控制后台任务。

#### 软件架构

>dotnet core的后台任务组件，控制台和web项目都可以直接用。

##### 工作中心`Worker`，定义了后台任务以什么样的流程来执行

- **OnceWorker** 调用一次执行一次的后台任务，可配置自定义数据。在Action里、服务里、任何其他位置调用. 配置BackRun来执行自己的业务逻辑，一个Worker可以配置多个BackRun。

- **QueueWorker** 队列任务，用QueueWorker添加string到队列，后台任务会立即执行。配置QueueBackRun来写自己的任务逻辑，请自己序列化String，一个Worker可以配置多个QueueBackRun。。
- **TimeWorker** 简单的周期执行任务，配置一个TimeSpan，周期循环执行定义的BackRun。继承BackRun/ScopeBackRun来写自己的任务逻辑。
- **PlanTimeWorker** 按时间计划执行的任务，配置[Cro表达式](#cro)（**Cron表达式的简化版**），在指定时间执行Backrun。继承BackRun/ScopeBackRun来写自己的任务逻辑。一个BackRun可以配置多个时间计划，一个时间计划可以对应多个不同类型的BackRun。

##### IBackRun：写业务逻辑的地方，需要自己继承实现抽象方法，不同的Worker可能要求不同的BackRun

- **BackRun** 最基础的执行逻辑，公开一个字典属性，在同一个Worker实例中每次执行时共享数据，OnceWorker和TimeWorker都使用这个。不同的BackRun可以配置给同一个OnceWorker。
- **QueueBackRun** QueueWorker独有的执行器，接受一个string类型的参数，为每次添加到队列的String数据，目前需要自己序列化。不同的QueueBackRun可以配置给同一个QueueWorke。
- **ScopeBackRun** 和BackRun类似，不过每次调用会创建属于自己的Ioc生命周期，GetService\<T>取对象时可以直接取services.AddScoped的服务(BackRun中需要自己NewScope)。

##### WorkerObserver：Worker的观察者（拦截器），目前只是简单的计数、Log，用户也能方便的添加自定义拦截器，后期会用他们实现持久化

##### WorkerContext：Worker的上下文，储存每个Worker实例运行时的数据，实时监控就是取它的数据

##### 安装教程

>nuget搜索Brun 或自己编译

#### 使用说明

1. 创建一个继承自BackRun的类

```csharp
//自己任务逻辑的例子，队列任务请继承QueueBackRun
public class TestHttpWorker : BackRun
    {
        public async override Task Run(CancellationToken stoppingToken)
        {

            for (int i = 0; i < 1; i++)
            {
                var log = GetRequiredService<ILogger<TestHttpWorker>>();
                
                HttpClient httpClient = new HttpClient();
                var r = await httpClient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("httpClient发起了请求,state:" + r.StatusCode);

                //也能使用自带的ioc获取服务，瞬时和单例可以直接取，Scoped的需要自己创建CreateScope
                IHttpClientFactory factory = GetRequiredService<IHttpClientFactory>();
                var fclient = factory.CreateClient();
                var fr = await fclient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("fclient发起了请求,state:" + fr.StatusCode);

                using (var scope = NewScope())
                {
                    var testScope = scope.ServiceProvider.GetRequiredService<ITestScopeService>();
                    string scopeR = testScope.Todo();
                    log.LogInformation(scopeR);
                }
            }
        }
    }
```

2. 服务配置

```csharp
public class Program
    {
        public static string BrunKey = Guid.NewGuid().ToString();
        public static string QueueKey = Guid.NewGuid().ToString();
        public static string TimeKey = Guid.NewGuid().ToString();
        public static string ScopeKey = Guid.NewGuid().ToString();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    //其他服务
                    services.AddHttpClient();
                    //services.AddScoped<ITestScopeService, TestScopeService>();

                    //配置单次任务
                    WorkerBuilder.Create<TestHttpWorker>()
                    .SetKey(BrunKey)
                    .Build();
                    
                    WorkerBuilder.Create<LongTimeBackRun>()
                    .Add<LongTimeBackRun>()//同一个OnceWorker中配置多个BackRun，使用：worker.RunDontWait<TBackRun>()
                    .SetName(nameof(LongTimeBackRun))
                    .Build();

                    //配置Scope任务
                    //WorkerBuilder.Create<TestScopeBackRun>()
                    //.SetKey(ScopeKey)
                    //.Build();


                    //配置队列任务
                    WorkerBuilder.CreateQueue<TestQueueWorker>()
                    .AddQueue<TestQueueErrorWorker>()//配置多个QueueBackRrun，使用:worker.Enqueue<TQueueBackRun>(msg)
                    .SetKey(QueueKey)
                    .Build();


                    //配置简单循环执行任务
                    //WorkerBuilder.CreateTime<ErrorTestRun>()
                    WorkerBuilder.CreateTime<LongTimeBackRun>()
                    .SetCycle(TimeSpan.FromSeconds(5), true)
                    .SetKey(TimeKey)
                    .Build()
                    ;

                    //配置复杂时间计划任务
                    WorkerBuilder.CreatePlanTime<LogTimeRun>("0/5 * * * *", "3,33,53 * * * *", "5 * * * *", "* * * * *")
                    .AddPlanTime<ErrorTestRun>("* * * * *")
                    .SetKey(PlanKey)
                    .Build();

                    //启动后台服务
                    services.AddBrunService();
                })
                ;
    }
```

3. Action中使用例子

```csharp
public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Brun.IWorkerServer _workerServer;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _workerServer = WorkerServer.Instance;//或者构造函数中用 IWorkerServer 取
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Once()
        {
            //运行后台任务
            _workerServer.GetOnceWorker(Program.BrunKey).RunDontWait();
            return View();
        }
        public IActionResult Queue(string msg)
        {
            //运行队列任务
            IQueueWorker worker = _workerServer.GetQueueWorker(Program.QueueKey);
            for (int i = 0; i < 100; i++)
            {
                worker.Enqueue(msg);
            }
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
```

4. 完整代码参考simples/BrunWebTest项目，DocFx生成的文档：[http://www.dotnet6.net/api/Brun.html](http://www.dotnet6.net/api/Brun.html)

#### <a name="cro">Cro表达式</a> ：Cron表达式的简化版
- 合并了日期和星期域，比Cron表达式少一个域，Cron表达式是6-7个域，Cro是5-6个。移除了第6个DayofWeek域
- 不支持第几个星期几这种计划，其他的基本都能满足
>**Cro域的格式，特殊字符不能是中文版**

| 秒(0-59) | 分(0-59) | 时(0-23) | 日/星期(1-31/1-7) | 月(1-12) | 年(1970-2099) |
| :-----| ----: | :----: |:----: |:----: |:----: |
| * , - / 数字 | * , - / 数字 | * , - / 数字 | * , - L 数字 X | * , - / 数字 | 可空 * , - / 数字 |

- **\*** 匹配任意，如秒域指定 \* 表示每秒都会执行, 年可空，默认也是*
- **\,** 数组匹配，如分钟域指定 0,10,40 表示0分，10分，40分钟的时候都满足条件
- **\-** 范围匹配，如小时域指定 10-17 表示10点到17点内都满足条件
- **\/** 步进匹配，如日期域指定 1/2 表示从1月开始（包含1月），每过2个月，那么3月，5月，7月...满足条件。\*\/2等同1\/2。如果是秒（初始是0）\*\/10 等同0\/10。还能和范围组合使用 3\-12\/3 表示在3-12月内，从3月开始，每过3个月满足条件。注意：星期暂时不支持步进（这种需求很少）。
- L 只能在日期域中使用，表示最后一天，2L表示最后一天再往前2天，30L在遇到不是31天的月份都会跳过当月。
- X 只能在日期域中使用，表示以星期判断，和日期互斥，X1表示每个星期天，X2表示星期一，X7表示星期六。X2-6表示星期一到星期五，X1,7表示周末。

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request