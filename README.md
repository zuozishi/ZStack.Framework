# ZStack快速开发框架

## 开始使用

### 使用脚手架创建项目


### 手动创建项目

#### 控制台程序

1. 添加NuGet程序包 `ZStack.Extensions`

2. Program.cs

```c#
global using Microsoft.Extensions.DependencyInjection;
global using Serilog;
global using ZStack.Core;

var sp = DependencyInjection.CreateConsoleAppServiceProvider(configure => { });

var logger = sp.GetRequiredService<ILogger>()
    .ForContext<Program>();

logger.Information("Hello, World!");
```

#### ASP .NET Core

1. 添加NuGet程序包 `ZStack.AspNetCore`

2. GlobalUsings.cs

```c#
global using Furion;
global using ZStack.AspNetCore;
global using App = ZStack.AspNetCore.App;
```

3. Program.cs

```c#
var builder = WebApplication.CreateBuilder(args).InjectZStack();

var app = builder.Build();

app.Run();
```

4. Startup.cs

```c#
/// <summary>
/// 应用启动配置
/// </summary>
public class Startup : AppStartup
{
    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews().AddInject();
    }

    /// <summary>
    /// 中间件配置
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        ...
        app.UseRouting();
        ...

        app.UseZStackInject();
    }
}
```


