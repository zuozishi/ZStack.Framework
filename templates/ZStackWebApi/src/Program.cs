var builder = WebApplication.CreateBuilder(args)
    .Inject();

var app = builder.Build();

app.UseZStackInject();

app.Run();
