var builder = WebApplication.CreateBuilder(args).InjectZStack();

var app = builder.Build();

app.Run();
