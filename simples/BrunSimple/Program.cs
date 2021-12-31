using Brun;
using BrunUI;

var builder = WebApplication.CreateBuilder(args);

//if (Environment.OSVersion.Platform == PlatformID.Unix)
//{
//    builder.WebHost.ConfigureKestrel(options =>
//    {
//        options.ListenUnixSocket("/tmp/kestrel-test.sock");
//    });
//}


// Add services to the container.
builder.Services.AddControllersWithViews();

//前端开发环境配置
builder.Services.AddCors(options =>
{
    options.AddPolicy("brun", builder =>
    {
        builder.WithOrigins("http://localhost:8000")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});
builder.Services.AddBrunService(options =>
{
    //options.UseRedis("192.168.1.4");
    options.UseInMemory();
    //options.UseStore(builder.Configuration.GetConnectionString("brun"), DbType.PostgreSQL);
    //程序启动时创建Worker和BackRun
    options.InitWorkers = async workerService =>
    {
        //添加Worker
        IOnceWorker onceworker = await workerService.AddOnceWorker(new WorkerConfig("once_1", ""));
        var queueWorker = await workerService.AddQueueWorker(new WorkerConfig("queue_1", ""));
        var timeWorker = await workerService.AddTimeWorker(new WorkerConfig());
        var planWorker = await workerService.AddPlanWorker(new WorkerConfig());
        //添加BackRun
        await onceworker.AddBrun<BrunTestHelper.BackRuns.AwaitErrorBackRun>(new OnceBackRunOption("onceB_1", "onceB_Name"));
        await queueWorker.AddBrun<BrunTestHelper.QueueBackRuns.LogQueueBackRun>(new QueueBackRunOption());
        await timeWorker.AddBrun<BrunTestHelper.LogTimeBackRun>(new TimeBackRunOption(TimeSpan.FromSeconds(5)));
        await planWorker.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(new PlanTime("0 * * * *")));
        await planWorker.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(PlanTime.Create("0/5 * * * *")));
    };
}).AddBrunUI(authoptions =>//启动UI组件
{
    authoptions.AuthType = AuthType.SimpleToken;
    authoptions.UserName = "admin";
    authoptions.Password = "admin";
    //或者appsetting中配置选项，key：BrunAuthenticationScheme，文档：https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0#use-ioptionssnapshot-to-read-updated-data
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    //app.UseHttpLogging();
}

app.UseStaticFiles();

app.UseBrunUI();

app.UseRouting();

//前端开发环境配置
app.UseCors("brun");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
