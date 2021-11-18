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

builder.Services.AddBrunService(workerServer =>
{
    //����˲ʱ����
    workerServer.CreateOnceWorker(new WorkerConfig()
    {
        Key = "t1",
        Name = "name1",
    }).AddData(new ConcurrentDictionary<string, string>()).AddBrun<LogBackRun>();

    //����ѭ������
    workerServer.CreateTimeWorker(new WorkerConfig("time_1", "time_name")).AddBrun(typeof(BrunTestHelper.LogTimeBackRun), new TimeBackRunOption(TimeSpan.FromSeconds(10)));

    //������Ϣ����
    workerServer.CreateQueueWorker(new WorkerConfig("q_1", "q_name")).AddBrun(typeof(BrunTestHelper.QueueBackRuns.LogQueueBackRun), new QueueBackRunOption()
    {
        Id = Guid.NewGuid().ToString()
    });

    //���üƻ�����
    workerServer.CreatePlanTimeWorker(new WorkerConfig("p_1", "p_name")).AddBrun(typeof(BrunTestHelper.LogPlanBackRun), new Brun.Options.PlanBackRunOption() { PlanTime = new Brun.Plan.PlanTime("0/5 * * * *") });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.Map("/brun", app =>
{
    app.UseBrunUI();
});
//app.UseBrunUI();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
