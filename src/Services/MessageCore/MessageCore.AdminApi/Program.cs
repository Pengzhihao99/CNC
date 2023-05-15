using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Framework.Repository.MongoDB.Models;
using MessageCore.AdminApi.Extensions;
using MessageCore.Infrastructure.Adapter.AutoMapper;
using MessageCore.Infrastructure.Ioc.AutofacModule;
using MessageCore.Infrastructure.Service.Configure.Extensions.Database.MongoDB;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

Log.Logger = new LoggerConfiguration()
               //.WriteTo.Console()
               .CreateBootstrapLogger();

Log.Information("Starting up!");
try
{
    var builder = WebApplication.CreateBuilder(args);

    var esUri = builder.Configuration.GetValue<string>("Elasticsearch:nodeUris");
    var indexFormat = builder.Configuration.GetValue<string>("Elasticsearch:indexFormat");
    var userName = builder.Configuration.GetValue<string>("Elasticsearch:userName");
    var password = builder.Configuration.GetValue<string>("Elasticsearch:password");

    builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(SerilogConfiguration())
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(esUri))
                    {
                        TypeName = null,
                        IndexFormat = indexFormat,
                        EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        ModifyConnectionSettings =
                            conn =>
                            {
                                conn.ServerCertificateValidationCallback((source, certificate, chain, sslPolicyErrors) => true);
                                conn.BasicAuthentication(userName, password);
                                return conn;
                            }
                    })
                    //.WriteTo.Console()
                    );
    //autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

    // Add services to the container.
    builder.Services.AddControllers().AddControllersAsServices().AddNewtonsoftJson();
    //builder.Services.AddControllers();
    builder.Services.AddHealthChecks();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    //跨域
    //builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
    //              .AllowAnyMethod()
    //              .AllowAnyHeader()));

    //添加AutoMapper的支持
    builder.Services.AddAutoMapper(typeof(CommonProfile).Assembly);

    //Configure
    builder.Services.Configure<MongoDBConnectionOptions>(builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForWrite", builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForRead", builder.Configuration.GetSection("MongoDBForRead"));

    var app = builder.Build();

    //Migration
    app.ConfigMongoDB(true);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }

    //异常处理中间件
    app.UseMiddleware<ExceptionHttpHandlerMiddleware>();

    //

    //app.UseAuthorization();
    //启动允许跨域访问
    //app.UseCors("AllowAll");

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

//Autofac注册容器
static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
{
    builder.RegisterModule(new RepositoryModule());
    builder.RegisterModule(new ApplicationModule());
    builder.RegisterModule(new CommonModule());
    builder.RegisterModule(new MediatorModule());
    builder.RegisterModule(new DomainModule());
}
