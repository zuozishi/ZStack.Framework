# ZStack.AspNetCore.Hangfire.Redis

Hangfire Redis 存储组件，使用 StackExchange.Redis。

## 使用

组件会通过 `HangfireRedisComponent` 自动注册。

配置：

```json
{
  "Hangfire": {
    "ConnectionString": "localhost:6379,defaultDatabase=0",
    "Redis": {
      "Prefix": "hangfire:",
      "InvisibilityTimeout": "00:30:00"
    },
    "Server": {
      "WorkerCount": 20
    }
  }
}
```

`Redis` 节点对应 `Hangfire.Redis.StackExchange.RedisStorageOptions`。

## 手动注册

```csharp
services.AddZStackHangireRedisStorage(options =>
{
    // 额外 Hangfire 配置
});
```
