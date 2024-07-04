using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using cartservice;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .ConfigureServices(services =>
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("cartservice"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("https://tempo.grafana.net:443");
                        options.Headers = "Authorization=Basic YOUR_GRAFANA_TEMPO_USERNAME:YOUR_GRAFANA_TEMPO_PASSWORD";
                    });
            });
        });
