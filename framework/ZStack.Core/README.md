# ZStack.Core

ZStack框架核心库，提供依赖注入、日志、配置、异常处理等基础能力。

## 应用程序主机构建器

控制台程序入口：

```csharp
var sp = AppHostBuilder.CreateHostBuilder(args).Build().Services;
var logger = sp.GetRequiredService<ILogger<Program>>();
logger.Information("Hello, World!");
```

## 依赖注入

### 自动注入

通过 `[Injection]` 特性标记类，配合生命周期接口自动注册：

```csharp
// 标记为可注入，默认自注入模式
[Injection]
public class MyService : ISingleton  // 单例
{
}

[Injection(Action = InjectionActions.TryAdd, Pattern = InjectionPatterns.FirstInterface)]
public class MyRepository : IScoped, IMyRepository  // 作用域
{
}

[Injection(Pattern = InjectionPatterns.ImplementedInterfaces)]
public class MyUtil : ITransient  // 瞬态
{
}
```

**注册模式（InjectionPatterns）：**
- `Self` — 仅注册自身类型
- `FirstInterface` — 注册第一个实现的接口
- `SelfWithFirstInterface` — 同时注册自身和第一个接口
- `ImplementedInterfaces` — 注册所有实现的接口
- `All` — 注册自身和所有接口

**生命周期接口：** `ISingleton`、`IScoped`、`ITransient`

**手动触发自动注入：**

```csharp
services.AddAutoDependencyInjection();
```

### 注册配置选项

```csharp
// 自动根据类名推断配置节名称（如 MyAppOptions → "MyApp"）
services.AddZStackOptions<MyAppOptions>();

// 指定配置节名称
services.AddZStackOptions<MyAppOptions>("CustomSection");
```

使用 `[OptionsSection("Key")]` 特性可自定义配置节绑定名称。

### Scoped 作用域

```csharp
Scoped.Create(async (scopeFactory, scope) =>
{
    var service = scope.ServiceProvider.GetRequiredService<MyService>();
    // 执行操作
}, scopeFactory);
```

## 日志（Serilog）

```csharp
// 创建控制台日志记录器
var logger = SerilogLogger.CreateConsoleAppLogger(configure =>
{
    configure.WriteTo.File("logs/app.log");
});

// 从配置文件创建日志记录器（自动加载 logger.json）
var logger = SerilogLogger.CreateConfigurationLogger(configure => { });

// 从指定配置文件前缀创建
var logger = SerilogLogger.CreateConfigurationLogger(configure => { }, "serilog");

// 从已有 IConfigurationRoot 创建
var logger = SerilogLogger.CreateConfigurationLogger(configuration, configure => { });
```

## 配置

### 自动加载配置目录

框架启动时自动扫描 `Configuration/` 目录，加载 JSON、INI、YAML 配置文件，并自动筛选环境特定文件（如 `app.Development.json`）。

```csharp
// 手动添加配置目录
configuration.AddDirectory(configDirectory, environment);
```

### 获取配置对象

```csharp
// 绑定配置节到对象
var options = configuration.GetOtions<MyOptions>();
```

## 异常处理

```csharp
// 抛出业务异常（状态码 400）
throw Oops.Bah("用户名已存在");

// 抛出系统异常（状态码 500）
throw Oops.Oh("服务内部错误");

// 抛出自定义状态码异常
throw Oops.Throw(404, "资源未找到");
throw Oops.Throw(HttpStatusCode.Forbidden, "无权限");
```

**注意：** `Oops` 方法返回异常对象，需要配合 `throw` 使用才能抛出。

## 性能追踪

```csharp
using (PerformanceTracker.CreateScope(elapsed =>
{
    Console.WriteLine($"耗时: {elapsed.TotalMilliseconds}ms");
}))
{
    // 耗时操作
}
```

## 其他工具

### RefAsync

可变引用包装类型，支持 `T` 与 `RefAsync<T>` 的隐式转换：

```csharp
RefAsync<int> counter = 0;
counter.Value++;
```

### TempFile

临时文件管理，自动在 `Dispose` 时删除：

```csharp
using var temp = new TempFile("文件内容");
// 使用 temp.FilePath ...
// 自动删除
```

### JSON 序列化转换器

- `DateTimeConverter` — 自定义格式的 DateTime 序列化
- `TimeStampConverter` — Unix 时间戳与 DateTime 互转

### Cache 扩展

```csharp
// 模糊搜索缓存Key（支持?和*通配符）
cache.Search("User:*");
```
