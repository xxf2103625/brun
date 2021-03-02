# Brun

#### 介绍
netcore 轻量级后台任务，asp.net中简单易用。

目前仅支持内存运行，后期可能会支持持久化

1. OnceWorker：调用第一执行一次的后台任务，可配置自定义数据。在Action里、服务里、任何其他位置调用. 继承BackRun来写自己的任务逻辑。
2. QueueWorker: 队列任务，QueueBackRun接收一个string，任何位置可传入一个string到队列，后台任务会立即执行。继承QueueBackRun来写自己的任务逻辑，请自己序列化String。
3. TimeWorker: 简单的定时任务，配置一个TimeSpan，周期执行定义的BackRun。继承BackRun来写自己的任务逻辑。

#### 软件架构
基于netcore IHostedService 的后台任务组件

控制台和web项目都可以直接用

#### 安装教程

nuget搜索Brun 或自己编译

#### 使用说明

1. 创建一个继承自BackRun的类

```csharp
//自己任务逻辑的例子，队列任务继承QueueBackRun
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
                IHttpClientFactory factory= GetRequiredService<IHttpClientFactory>();
                var fclient= factory.CreateClient();
                var fr= await fclient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("fclient发起了请求,state:" + fr.StatusCode);

                using (var scope = CreateScope())
                {
                    HttpClient iocClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
                    var iocR = await iocClient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                    log.LogWarning("iocClient发起了请求,state:" + iocR.StatusCode);
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
                    services.AddScoped<ITestScopeService, TestScopeService>();

                    //配置单次任务
                    WorkerBuilder.Create<TestHttpWorker>()
                    .SetKey(BrunKey)
                    .Build();

                    //配置队列任务
                    WorkerBuilder.CreateQueue<TestQueueWorker>()
                    .SetKey(QueueKey)
                    .Build();


                    //配置定时任务
                    WorkerBuilder.CreateTime<ErrorTestRun>()
                    .SetCycle(TimeSpan.FromSeconds(3))
                    .SetKey(TimeKey)
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
        public async Task<IActionResult> Queue(string msg)
        {
            //运行队列任务
            IQueueWorker worker = _workerServer.GetQueueWorker(Program.QueueKey);
            for (int i = 0; i < 100; i++)
            {
                await worker.Enqueue(msg);
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

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


