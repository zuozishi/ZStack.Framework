var sp = DependencyInjection.CreateConsoleAppServiceProvider(configure => { });

var logger = sp.GetRequiredService<ILogger>()
    .ForContext<Program>();

logger.Information("Hello, World!");