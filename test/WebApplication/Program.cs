var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args).Inject();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.UseZStackInject();

app.Run();
