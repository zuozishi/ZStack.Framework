using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args).Inject();

builder.Services.AddRazorPages();

builder.Services.ConfigureOpenTelemetryTracerProvider(p =>
{
    p.AddSource("SqlSugar");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseZStackInject();

app.Run();
