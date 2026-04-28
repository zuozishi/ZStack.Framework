# ZStack.AspNetCore.Hangfire.MemoryStorage

Hangfire 内存存储组件，适用于开发/测试环境。

## 使用

组件会通过 `HangfireMemoryComponent` 自动注册。

配置：

```json
{
  "Hangfire": {
    "MemoryStorage": {
      "CountersAggregationInterval": "00:05:00"
    },
    "Server": {
      "WorkerCount": 2
    }
  }
}
```

`MemoryStorage` 节点对应 `Hangfire.MemoryStorage.MemoryStorageOptions`。

## 手动注册

```csharp
services.AddZStackHangireMemoryStorage(options =>
{
    // 额外 Hangfire 配置
});
```
