using Autofac;
using Autofac.Extensions.DependencyInjection;
using Framework.Repository.MongoDB.Models;
using MessageCore.OpenApi.Extensions;
using MessageCore.Infrastructure.Adapter.AutoMapper;
using MessageCore.Infrastructure.Ioc.AutofacModule;
using MessageCore.Infrastructure.Service.Configure.Extensions.Database.MongoDB;
using Serilog;
using Microsoft.OpenApi.Models;

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
    builder.Services.AddControllers().AddControllersAsServices().AddNewtonsoftJson();
    builder.Services.AddHealthChecks();
    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MessageCore Open API", Version = "v1" });
        var apifilePath = Path.Combine(System.AppContext.BaseDirectory, @"MessageCore.OpenApi.xml");
        var applicationfilePath = Path.Combine(System.AppContext.BaseDirectory, @"MessageCore.Application.xml");
        c.IncludeXmlComments(apifilePath);
        c.IncludeXmlComments(applicationfilePath);
        //UseFullTypeNameInSchemaIds replacement for .NET Core
        //c.CustomSchemaIds(x => x.FullName);
    });


    //添加AutoMapper的支持
    builder.Services.AddAutoMapper(typeof(CommonProfile).Assembly);

    //Configure
    builder.Services.Configure<MongoDBConnectionOptions>(builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForWrite", builder.Configuration.GetSection("MongoDBForWrite"));
    builder.Services.Configure<MongoDBConnectionOptions>("MongoDBForRead", builder.Configuration.GetSection("MongoDBForRead"));

    var app = builder.Build();

    //Migration
    app.ConfigMongoDB(false);

    //异常处理中间件
    app.UseMiddleware<ExceptionHttpHandlerMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }

    //app.UseAuthorization();

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

