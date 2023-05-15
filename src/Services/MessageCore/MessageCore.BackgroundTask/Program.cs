using Hangfire;
using Hangfire.Redis;
using MessageCore.BackgroundTask.Extensions;
using MessageCore.BackgroundTask.Jobs;
using StackExchange.Redis;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using MessageCore.Infrastructure.Ioc.AutofacModule;
using Microsoft.AspNetCore.Mvc.Filters;
using MessageCore.Infrastructure.Service.Configure.Extensions.Database.MongoDB;
using Framework.Repository.MongoDB.Models;

Log.Logger = new LoggerConfiguration()
               //.WriteTo.Console()
               .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
                   .ReadFrom.Configuration(SerilogConfiguration())
                   .ReadFrom.Services(services)
                   .Enrich.FromLogContext()
                   //.WriteTo.Console()
                   );
    //autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();
    builder.Services.AddHangfire(configuration =>
    {
        var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("HangfireRedisConnection:ConnectionString"));
        configuration.UseRedisStorage(redis, new RedisStorageOptions()
        {
            Db = builder.Configuration.GetValue<int>("HangfireRedisConnection:Database"),
            SucceededListSize = 1000,
            DeletedListSize = 1000,
            Prefix = builder.Configuration.GetValue<string>("HangfireRedisConnection:Prefix")
        });

        configuration.UseSerilogLogProvider();
    });

    builder.Services.AddHangfireServer((options =>
    {
        options.ServerName = string.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString());
        options.WorkerCount = 5;
        options.SchedulePollingInterval = TimeSpan.FromSeconds(4);
       
    }));

    //Configure
    builder.Services.Configure<MongoDBConnectionOptions>(builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForWrite", builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForRead", builder.Configuration.GetSection("MongoDBForRead"));

    var app = builder.Build();

    //Migration
    app.ConfigMongoDB(false);

    // Configure the HTTP request pipeline.

    //Æô¶¯Hangfire UI
    app.UseHangfireDashboard("/hangfire", new DashboardOptions()
    {
        Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
    });

    //ÅäÖÃjob
    HangfireJobManager.ConfigJob();

    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.MapControllers();
    app.MapHealthChecks("/hc");

    app.Run();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static IConfiguration SerilogConfiguration()
{
    return new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("./Config/serilog.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();
}

//Autofac×¢²áÈÝÆ÷
static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
{
    builder.RegisterModule(new RepositoryModule());
    builder.RegisterModule(new ApplicationModule());
    builder.RegisterModule(new CommonModule());
    builder.RegisterModule(new MediatorModule());
    builder.RegisterModule(new DomainModule());
}