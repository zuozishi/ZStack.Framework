using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args)
    .Inject();

builder.Services.AddRazorPages();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
//        options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
//    });

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "zstack",
            ValidAudience = "zstack",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zstack")),
        };
    });
builder.Services.AddMiniProfiler(options =>
{
    // 减少非 Swagger 页面请求监听问题
    options.ShouldProfile = (req) =>
    {
        if (!req.Headers.ContainsKey("request-from")) return false;
        return true;
    };

    options.RouteBasePath = "/index-mini-profiler";
    options.EnableMvcFilterProfiling = false;
    options.EnableMvcViewProfiling = false;
});
builder.Services.Configure<SwaggerUIOptions>(options =>
{
    var thisAssembly = typeof(Program).Assembly;
    options.IndexStream = () =>
    {
        StringBuilder htmlBuilder;
        // 自定义首页模板参数
        var indexArguments = new Dictionary<string, string>
            {
                {"%(VirtualPath)", "" }    // 解决二级虚拟目录 MiniProfiler 丢失问题
            };

        // 读取文件内容
        using (var stream = File.OpenRead("index-mini-profiler.html"))
        {
            using var reader = new StreamReader(stream);
            htmlBuilder = new StringBuilder(reader.ReadToEnd());
        }

        // 替换模板参数
        foreach (var (template, value) in indexArguments)
        {
            htmlBuilder.Replace(template, value);
        }

        // 返回新的内存流
        var byteArray = Encoding.UTF8.GetBytes(htmlBuilder.ToString());
        return new MemoryStream(byteArray);
    };
    // 配置多语言和自动登录token
    options.UseRequestInterceptor("function(request) { return defaultRequestInterceptor(request); }");
    options.UseResponseInterceptor("function(response) { return defaultResponseInterceptor(response); }");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseUnifyResultStatusCodes();
app.UseMiniProfiler();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.UseZStackInject();

app.Run();
