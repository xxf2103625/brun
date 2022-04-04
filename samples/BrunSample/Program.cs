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

//ǰ�˿�����������
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.WithOrigins(builder.Configuration.GetValue<string>("Cors"))
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});


builder.Services.AddBrunService(options =>
{
    //TODO ��ʼ��������������
    Brun.Commons.BrunTool.LoadFile("BrunTestHelper.dll");
    //options.UseRedis("192.168.1.4");
    options.UseInMemory();
    //options.UseStore(builder.Configuration.GetConnectionString("brun"), DbType.PostgreSQL);
    //��������ʱ����Worker��BackRun
    options.InitWorkers = workerService =>
    {
        //���Worker
        IOnceWorker onceworker = workerService.AddOnceWorker(new WorkerConfig("once_1", ""));
        var queueWorker = workerService.AddQueueWorker(new WorkerConfig("queue_1", ""));
        var timeWorker = workerService.AddTimeWorker(new WorkerConfig());
        var planWorker = workerService.AddPlanWorker(new WorkerConfig());
        //���BackRun
        onceworker.AddBrun<BrunTestHelper.BackRuns.AwaitErrorBackRun>(new OnceBackRunOption("onceB_1", "onceB_Name"));
        queueWorker.AddBrun<BrunTestHelper.QueueBackRuns.LogQueueBackRun>(new QueueBackRunOption());
        timeWorker.AddBrun<BrunTestHelper.LogTimeBackRun>(new TimeBackRunOption(TimeSpan.FromSeconds(5)));
        planWorker.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(new PlanTime("0 * * * *")));
        planWorker.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(PlanTime.Create("0/5 * * * *")));
    };
}).AddBrunUI(authoptions =>//����UI���
{
    authoptions.AuthType = AuthType.SimpleToken;
    authoptions.UserName = "admin";
    authoptions.Password = "admin";
    //����appsetting������ѡ�key��BrunAuthenticationScheme���ĵ���https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0#use-ioptionssnapshot-to-read-updated-data
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseHttpLogging();
}

app.UseStaticFiles();

app.UseBrunUI();

app.UseRouting();

//ǰ�˿�����������
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
