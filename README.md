# Brun

#### 介绍
netcore 轻量级后台任务，asp.net中简单易用。

#### 软件架构
基于netcore IHostedService 的后台任务组件

控制台和web项目都可以直接用

#### 安装教程

nuget搜索Brun 或自己编译

#### 使用说明

1. 创建一个继承自BackRun的类

```csharp
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
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    //其他服务
                    services.AddHttpClient();
                    //配置任务
                    WorkerBuilder.Create<TestHttpWorker>()
                    .SetKey(BrunKey)
                    .Build();
                    //启动后台服务
                    services.AddBrunService();
                })
                ;
    }
``` 

3. Action中使用 

```csharp
public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Brun.IWorkerServer _workerServer;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _workerServer = WorkerServer.Instance;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  IActionResult Privacy()
        {
            //运行后台任务
            _workerServer.GetWorker(Program.BrunKey).RunDontWait();
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


