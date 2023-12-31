# ZStack快速开发框架

.NET基础库： ![Nuget](https://img.shields.io/nuget/v/ZStack.Core?label=ZStack.Core) ![Nuget](https://img.shields.io/nuget/v/ZStack.Extensions?label=ZStack.Extensions)

ASP .NET Core基础库： ![Nuget](https://img.shields.io/nuget/v/ZStack.AspNetCore?label=ZStack.AspNetCore)

ASP .NET Core组件库：![Nuget](https://img.shields.io/nuget/v/ZStack.AspNetCore.SqlSugar?label=ZStack.AspNetCore.SqlSugar) ![Nuget](https://img.shields.io/nuget/v/ZStack.AspNetCore.EventBus?label=ZStack.AspNetCore.EventBus) ![Nuget](https://img.shields.io/nuget/v/ZStack.AspNetCore.Hangfire?label=ZStack.AspNetCore.Hangfire)

## 1. 开始使用

### 1.1 使用脚手架创建项目


### 1.2 手动创建项目

#### *控制台程序*

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

#### *ASP .NET Core*

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

## 2. 项目结构

```mermaid
graph TD;
    ZStack.Core --> ZStack.Extensions;
    ZStack.Extensions --ASP .NET Core--> ZStack.AspNetCore;
    ZStack.AspNetCore --> ZStack.AspNetCore.SqlSugar;
    ZStack.AspNetCore --> ZStack.AspNetCore.EventBus;
    ZStack.AspNetCore --> ZStack.AspNetCore.Hangfire;
    ZStack.AspNetCore.Hangfire --> ..MemoryStorage;
    ZStack.AspNetCore.Hangfire --> ..Redis;
    ZStack.AspNetCore.Hangfire --> ..PostgreSql;
```

* [ZStack.Core](./framework/ZStack.Core/)：ZStack框架 核心库
* [ZStack.Extensions](./framework/ZStack.Extensions/)：ZStack框架 拓展方法库
* [ZStack.AspNetCore](./framework/ZStack.AspNetCore/)：ZStack框架 ASP .NET Core基础库
