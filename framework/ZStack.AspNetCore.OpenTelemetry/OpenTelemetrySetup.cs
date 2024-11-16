using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;
using ZStack.AspNetCore;
using ZStack.AspNetCore.OpenTelemetry;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenTelemetrySetup
{
    public static IOpenTelemetryBuilder AddZStackOpenTelemetry(this IServiceCollection services)
    {
        services.AddZStackOptions<OpenTelemetryOptions>();
        var options = App.GetOptions<OpenTelemetryOptions>();
        var entryAssemblyName = Assembly.GetEntryAssembly()!.GetName();
        options.ServiceName ??= App.HostEnvironment?.ApplicationName ?? entryAssemblyName.Name;
        options.ServiceVersion ??= entryAssemblyName.Version?.ToString();
        var builder = services.AddOpenTelemetry()
            .ConfigureResource(config =>
            {
                config.AddService(options.ServiceName!, options.ServiceNameSpace, options.ServiceVersion);
            })
            .WithMetrics(builder =>
            {
                builder.AddRuntimeInstrumentation();
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation();
                builder.AddMeter([.. options.Metrics]);
            })
            .WithTracing(builder =>
            {
                builder.SetSampler(new AlwaysOnSampler());
                builder.AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                });
                builder.AddHttpClientInstrumentation();
                builder.AddSource([.. options.Traces]);
            });
        var useOtlpExporter = !string.IsNullOrWhiteSpace(App.Configuration!["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtlpExporter)
            builder.UseOtlpExporter();
        return builder;
    }
}
