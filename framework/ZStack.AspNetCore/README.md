# ZStack.AspNetCore

ZStack框架 ASP.NET Core 基础库，提供组件化架构、规范化结果、OpenAPI 集成、应用启动器等能力。

## 快速开始

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args).Inject();
var app = builder.Build();
app.UseZStackInject();
app.Run();
```

`Inject()` 自动完成：Serilog 配置、组件发现与加载、依赖注入自动注册。`UseZStackInject()` 执行所有中间件组件和 Startup 配置。

## 组件系统

组件是框架的核心扩展机制。实现以下接口创建组件：

### IServiceComponent — 服务注册组件

```csharp
public class MyComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        // 注册服务
        services.AddSingleton<IMyService, MyService>();
    }
}
```

### IApplicationComponent — 中间件组件

标记接口，在 `UseZStackInject()` 时按 `[ComponentOrder]` 排序执行。组件类可通过构造函数注入 `IApplicationBuilder`、`IWebHostEnvironment` 等。

```csharp
[ComponentOrder(100)]
public class MyMiddlewareComponent : IApplicationComponent
{
    public void Load(IApplicationBuilder app, IWebHostEnvironment env, ComponentContext ctx)
    {
        app.UseMiddleware<MyMiddleware>();
    }
}
```

### 组件依赖

```csharp
[DependsOn(typeof(CacheComponent))]
public class DatabaseComponent : IServiceComponent { }
```

## AppStartup 启动器

继承 `AppStartup` 的类会被自动发现并执行：

```csharp
[AppStartup(Order = 1)]
public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthorization();
    }
}
```

## App 全局静态类

提供对服务容器、配置、日志等的便捷访问：

```csharp
// 获取服务
var service = App.GetRequiredService<IMyService>();
var service = App.GetService<IMyService>();

// 获取配置
var options = App.GetOptions<MyOptions>();
var config = App.Configuration;

// 获取请求上下文
var httpContext = App.HttpContext;
var user = App.User;

// 创建作用域
await App.UseScope(async sp =>
{
    var svc = sp.GetRequiredService<IMyService>();
});
```

## 规范化结果（UnifyResult）

统一的 API 返回格式：

```json
{
  "code": 200,
  "success": true,
  "result": { ... },
  "message": "success",
  "extras": null,
  "traceId": "0HN5A6H2VLBH6"
}
```

可通过实现 `IUnifyResultProvider` 接口自定义规范化结果格式。

## OpenAPI 集成

框架自动注册 OpenAPI/Scalar 文档。配置项：

```json
{
  "OpenApi": {
    "Enable": true,
    "EnableScalar": true,
    "Groups": {
      "v1": "API v1",
      "v2": "API v2"
    }
  }
}
```

## 缓存

支持内存缓存和 Redis 缓存：

```json
{
  "Cache": {
    "CacheType": "Redis",  // Memory 或 Redis
    "Redis": {
      "Server": "127.0.0.1:6379"
    }
  }
}
```

## 扩展方法

### HttpContext 扩展

```csharp
// 获取 Action 上的元数据特性
var attr = httpContext.GetMetadata<MyAttribute>();

// 判断是否是 WebSocket 请求
bool isWs = httpContext.IsWebSocketRequest();
```

### WebApplicationBuilder 扩展

```csharp
// 完整 Inject 参数
builder.Inject(
    loggerConfigure: config => { },
    autoLoadComponents: true,
    components: [typeof(CustomComponent)],
    ignoreComponents: [typeof(SomeComponent)]
);
```

### IApplicationBuilder 扩展

```csharp
app.UseZStackInject();       // 自动加载所有组件 + Startup
app.UseComponents();         // 仅加载组件
app.UseStartups();           // 仅加载 Startup
app.PrintAppEndpoints();     // 开发环境打印监听地址
```
