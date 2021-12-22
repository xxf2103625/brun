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

    //options.WorkerServer = options =>
    //{
    //    //����˲ʱ����
    //    options.CreateOnceWorker(new WorkerConfig()
    //    {
    //        Key = "t1",
    //        Name = "name1",
    //    }).AddData(new ConcurrentDictionary<string, string>()).AddBrun<LogBackRun>();

    //    //����ѭ������
    //    options.CreateTimeWorker(new WorkerConfig("time_1", "time_name")).AddBrun(typeof(BrunTestHelper.LogTimeBackRun), new TimeBackRunOption(TimeSpan.FromSeconds(10)));

    //    //������Ϣ����
    //    options.CreateQueueWorker(new WorkerConfig("q_1", "q_name")).AddBrun(typeof(BrunTestHelper.QueueBackRuns.LogQueueBackRun), new QueueBackRunOption()
    //    {
    //        Id = Guid.NewGuid().ToString()
    //    });

    //    //���üƻ�����
    //    options.CreatePlanTimeWorker(new WorkerConfig("p_1", "p_name")).AddBrun(typeof(BrunTestHelper.LogPlanBackRun), new Brun.Options.PlanBackRunOption() { PlanTime = new Brun.Plan.PlanTime("0/5 * * * *") });
    //};
}).AddBrunUI(authoptions =>
{
    authoptions.AuthType = BrunUI.Auths.BrunAuthType.BrunSimpleToken;
    authoptions.UserName = "brun";
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
app.UseCors("brun");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
