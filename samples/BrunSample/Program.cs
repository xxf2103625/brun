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
    options.InitWorkers = workers =>
    {
        //���Worker
        IOnceWorker once = workers.AddOnceWorker(new WorkerConfig("once_1", ""));
        IQueueWorker queue = workers.AddQueueWorker(new WorkerConfig("queue_1", ""));
        ITimeWorker time = workers.AddTimeWorker(new WorkerConfig());
        IPlanWorker plan = workers.AddPlanWorker(new WorkerConfig());
        //����BackRun
        once.AddBrun<BrunTestHelper.BackRuns.AwaitErrorBackRun>(new OnceBackRunOption("onceB_1", "onceB_Name"));
        queue.AddBrun<BrunTestHelper.QueueBackRuns.LogQueueBackRun>(new QueueBackRunOption());
        time.AddBrun<BrunTestHelper.LogTimeBackRun>(new TimeBackRunOption(TimeSpan.FromSeconds(5)));//5��ִ��һ��
        plan.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(new PlanTime("0 * * * *")));//ÿ����ִ��
        plan.AddBrun<BrunTestHelper.LogPlanBackRun>(new PlanBackRunOption(PlanTime.Create("0/5 * * * *")));//5��ִ��һ��
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
