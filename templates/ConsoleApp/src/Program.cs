using var host = AppHostBuilder.CreateHostBuilder(args).Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Hello from ZStack.Core console template!");
