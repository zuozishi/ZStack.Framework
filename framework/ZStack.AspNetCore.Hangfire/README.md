# ZStack.AspNetCore.Hangfire

基于 Hangfire 的后台任务调度组件，支持自动发现和注册定时任务。

## 快速开始

添加 `ZStack.AspNetCore.Hangfire` 及对应的存储组件包。任选一种存储：

- `ZStack.AspNetCore.Hangfire.MemoryStorage` — 内存存储（开发用）
- `ZStack.AspNetCore.Hangfire.Redis` — Redis 存储
- `ZStack.AspNetCore.Hangfire.PostgreSql` — PostgreSQL 存储

配置示例（Redis）：

```json
{
  "Hangfire": {
    "ConnectionString": "localhost:6379",
    "Server": {
      "WorkerCount": 5,
      "Queues": ["default", "critical"]
    },
    "Console": {
      "FollowResult": true
    }
  }
}
```

## 定义定时任务

继承 `HangfireJobBase<T>` 实现定时任务即可被自动发现和注册：

```csharp
public class CleanupJob : HangfireJobBase<CleanupJob>
{
    public override string JobId => "cleanup-task";
    public override string Cron => "0 2 * * *";  // 每天凌晨2点
    public override TimeZoneInfo TimeZone => TimeZoneInfo.Local;

    public override async Task RunAsync(PerformingContext? context, CancellationToken cancellationToken)
    {
        Logger.LogInformation("开始清理...");
        // 执行任务逻辑
    }
}
```

基类提供以下属性：

- `ServiceProvider` — 服务提供器
- `Logger` — 日志记录器
- `ProgressBarFactory` — 进度条工厂
- `JobManager` — Hangfire 任务管理器
- `PerformingContext` — 当前执行上下文

## 手动注册

```csharp
// 注册Hangfire（不自动添加存储）
services.AddZStackHangire(options, config =>
{
    config.UseSqlServerStorage(connectionString);
});

// 手动扫描注册定时任务
app.RegisterHangireJobs();
```
