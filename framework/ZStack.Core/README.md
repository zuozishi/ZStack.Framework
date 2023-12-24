# ZStack快速开发框架

## ZStack.Core【ZStack框架 核心库】

### 依赖注入

```c#
// 创建控制台应用程序依赖注入容器
DI.CreateConsoleAppServiceProvider(
    // 服务配置
    Action<ServiceCollection> servicesConfigure,
    // 添加日志
    bool addLogger = true,
    // 日志配置
    Action<LoggerConfiguration>? loggerConfigure = null
);
```

### 日志（Serilog）

```c#
// 创建控制台应用程序日志记录器
SerilogLogger.CreateConsoleAppLogger(
    // 日志配置
    Action<LoggerConfiguration>? configure = null,
    // 日志输出模板
    string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}][{ShortSourceContext}] {Message:lj}{NewLine}{Exception}"
);

// 创建通过配置文件配置的日志记录器
SerilogLogger.CreateConfigurationLogger(
    // 日志配置
    Action<LoggerConfiguration>? configure = null
    // 日志配置文件前缀
    , string filePrefix = "logger"
);

// 创建通过配置文件配置的日志记录器
SerilogLogger.CreateConfigurationLogger(
    IConfigurationRoot configuration,
    Action<LoggerConfiguration>? configure = null
);
```

### 性能跟踪器

```c#
using(PerformanceTracker.CreateScope(elapsed =>
{
    // 计时器结束
}))
{
    // 耗时操作
}

// 或

var tracker = PerformanceTracker.CreateTracker(elapsed =>
{
    // 计时器结束
});
// 耗时操作
tracker.Dispose();
```

### 工具类

**DataValidator.cs**

```c#
// 验证是否为有效的手机号
DataValidator.IsValidMobile(string mobile);

// 验证是否为有效的邮箱
DataValidator.IsValidEmail(string email);

// 验证是否为有效的身份证号
DataValidator.IsValidIdCard(string idCard);
```

**MD5.cs**

```c#
// 获取MD5字符串
MD5.GetMD5(string input);
MD5.GetMD5(byte[] bytes);
MD5.GetMD5(Stream stream);
```

**DataUtils.cs**

```c#
// 对比对象是否相等
bool IsEqual(object? value1, object? value2);
```