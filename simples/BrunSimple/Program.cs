using Brun;
using BrunUI;
using BrunTestHelper.BackRuns;
using System.Collections.Concurrent;

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


//builder.Services.
builder.Services.AddBrunService(options =>
{
    //options.UseRedis("192.168.1.8");
    //options.UseInMemory();
    options.UseStore(builder.Configuration.GetConnectionString("brun"), DbType.PostgreSQL);

    //options.WorkerServer = options =>
    //{
    //    //配置瞬时任务
    //    options.CreateOnceWorker(new WorkerConfig()
    //    {
    //        Key = "t1",
    //        Name = "name1",
    //    }).AddData(new ConcurrentDictionary<string, string>()).AddBrun<LogBackRun>();

    //    //配置循环任务
    //    options.CreateTimeWorker(new WorkerConfig("time_1", "time_name")).AddBrun(typeof(BrunTestHelper.LogTimeBackRun), new TimeBackRunOption(TimeSpan.FromSeconds(10)));

    //    //配置消息任务
    //    options.CreateQueueWorker(new WorkerConfig("q_1", "q_name")).AddBrun(typeof(BrunTestHelper.QueueBackRuns.LogQueueBackRun), new QueueBackRunOption()
    //    {
    //        Id = Guid.NewGuid().ToString()
    //    });

    //    //配置计划任务
    //    options.CreatePlanTimeWorker(new WorkerConfig("p_1", "p_name")).AddBrun(typeof(BrunTestHelper.LogPlanBackRun), new Brun.Options.PlanBackRunOption() { PlanTime = new Brun.Plan.PlanTime("0/5 * * * *") });
    //};
}).AddBrunUI(options =>
{
    options.AuthType = BrunUI.Auths.BrunAuthType.BrunSimpleToken;
    options.CheckUser = (userName, pwd) =>
    {
        if (userName == "admin" && pwd == "admin")
        {
            return true;
        }
        return false;
    };
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
