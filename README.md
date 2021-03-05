# Brun
### 介绍
#### netcore 轻量级后台任务，asp.net中简单易用。

目前仅支持内存运行，后期可能会支持持久化。


#### 软件架构
基于netcore IHostedService 的后台任务组件。

控制台和web项目都可以直接用。
#### IWorker：我把它定义为工作中心，定义了后台任务以什么样的流程来执行 
1. OnceWorker：调用一次执行一次的后台任务，可配置自定义数据。在Action里、服务里、任何其他位置调用. 继承BackRun来写自己的任务逻辑。
2. QueueWorker: 队列任务，用QueueWorker添加string到队列，后台任务会立即执行。继承QueueBackRun来写自己的任务逻辑，请自己序列化String。
3. TimeWorker: 简单的定时任务，配置一个TimeSpan，周期循环执行定义的BackRun。继承BackRun/ScopeBackRun来写自己的任务逻辑。

#### IBackRun：执行器，写业务逻辑的地方，需要自己继承，不同的Worker可能会有不同的上下文/参数
1. BackRun：最基础的执行逻辑，公开一个字典属性，在同一个Worker实例中每次执行时共享数据，OnceWorker和TimeWorker都使用这个。
2. QueueBackRun：QueueWorker独有的执行器，接受一个string类型的参数，为每次添加到队列的String数据，目前需要自己序列化。
3. ScopeBackRun：和BackRun类似，不过每次调用会创建属于自己的Ioc生命周期，从ServiceProvider取服务时可以直接取services.AddScoped的服务(BackRun中需要自己NewScope)。
#### WorkerObserver：Worker的观察者（拦截器），目前只是简单的计数、Log，用户也能方便的添加自定义拦截器（以后版本可能会换成Listener），后期会用他们实现持久化。
#### WorkerContext：Worker的上下文，储存每个Worker实例运行时的数据。

#### 安装教程

nuget搜索Brun 或自己编译

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
                    .SetName(nameof(LongTimeBackRun))
                    .Build();

                    //配置Scope任务
                    //WorkerBuilder.Create<TestScopeBackRun>()
                    //.SetKey(ScopeKey)
                    //.Build();


                    //配置队列任务
                    WorkerBuilder.CreateQueue<TestQueueWorker>()
                    .SetKey(QueueKey)
                    .Build();


                    //配置定时任务
                    //WorkerBuilder.CreateTime<ErrorTestRun>()
                    WorkerBuilder.CreateTime<LongTimeBackRun>()
                    .SetCycle(TimeSpan.FromSeconds(5), true)
                    .SetKey(TimeKey)
                    .Build()
                    ;

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
#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


