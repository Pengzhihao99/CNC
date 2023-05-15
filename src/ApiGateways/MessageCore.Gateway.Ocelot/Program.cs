using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Serilog;
using Ocelot.Values;
using System.ComponentModel;

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

    builder.WebHost
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddOcelot("OcelotConfig", hostingContext.HostingEnvironment)//ºÏ²¢ÅäÖÃ,Æ¥Åä(?i)ocelot.([a-zA-Z0-9]*).json 
                .AddEnvironmentVariables();
        })
        .UseIISIntegration();


    // Add services to the container.
    builder.Services.AddOcelot();
    builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        .AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddHealthChecks();


    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    //Æô¶¯ÔÊÐí¿çÓò·ÃÎÊ
    app.UseCors("AllowAll");

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc");
    });

    app.UseOcelot().Wait();

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


