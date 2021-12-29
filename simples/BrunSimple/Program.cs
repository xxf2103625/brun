using Brun;
using BrunUI;
using BrunTestHelper.BackRuns;
using System.Collections.Concurrent;
using Brun.Workers;

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
    //options.UseRedis("192.168.1.8");
    options.UseInMemory();
    //options.UseStore(builder.Configuration.GetConnectionString("brun"), DbType.PostgreSQL);

    options.InitWorkers = async workerService =>
    {
        IOnceWorker onceworker = await workerService.AddOnceWorker(new WorkerConfig("once_1", ""));
        var queueWorker = await workerService.AddQueueWorker(new WorkerConfig("queue_1", ""));
        var timeWorker = await workerService.AddTimeWorker(new WorkerConfig());
        var planWorker = await workerService.AddPlanWorker(new WorkerConfig());

        //TODO AddBrun迁移到Service 支持持久化
        onceworker.AddBrun<BrunTestHelper.BackRuns.AwaitErrorBackRun>(new Brun.Options.OnceBackRunOption("onceB_1", "onceB_Name"));
    };
}).AddBrunUI(authoptions =>
{
    authoptions.AuthType = BrunUI.Auths.BrunAuthType.BrunSimpleToken;
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
app.UseCors("brun");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
