using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;
using ZStack.AspNetCore;
using ZStack.AspNetCore.OpenTelemetry;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenTelemetrySetup
{
    public static IServiceCollection AddZStackOpenTelemetry(this IServiceCollection services)
    {
        services.AddZStackOptions<OpenTelemetryOptions>();
        var options = App.GetOptions<OpenTelemetryOptions>();
        var entryAssemblyName = Assembly.GetEntryAssembly()!.GetName();
        options.ServiceName ??= App.HostEnvironment?.ApplicationName ?? entryAssemblyName.Name;
        options.ServiceVersion ??= entryAssemblyName.Version?.ToString();
        services.AddOpenTelemetry()
            .ConfigureResource(config =>
            {
                config.AddService(options.ServiceName!, options.ServiceNameSpace, options.ServiceVersion);
            })
            .WithMetrics(builder =>
            {
                builder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
                builder.AddHttpClientInstrumentation();
                builder.AddRuntimeInstrumentation();
                builder.AddAspNetCoreInstrumentation();
                builder.AddMeter([.. options.Metrics]);
                foreach (var exporter in options.OtlpExporters)
                    builder.AddOtlpExporter(exporter.Name, otlpConfig =>
                    {
                        otlpConfig.Protocol = exporter.Protocol;
                        otlpConfig.Endpoint = new Uri(exporter.Endpoint);
                    });
            })
            .WithTracing(builder =>
            {
                builder.AddSource([.. options.Traces]);
                builder.SetSampler(new AlwaysOnSampler());
                builder.AddHttpClientInstrumentation();
                builder.AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                });
                foreach (var exporter in options.OtlpExporters)
                    builder.AddOtlpExporter(exporter.Name, otlpConfig =>
                    {
                        otlpConfig.Protocol = exporter.Protocol;
                        otlpConfig.Endpoint = new Uri(exporter.Endpoint);
                    });
            });
        return services;
    }
}
