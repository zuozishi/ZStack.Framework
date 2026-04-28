# ZStack.AspNetCore.Hangfire.PostgreSql

Hangfire PostgreSQL 存储组件，使用 Npgsql。

## 使用

组件会通过 `HangfirePostgreSqlComponent` 自动注册。

配置：

```json
{
  "Hangfire": {
    "ConnectionString": "Host=localhost;Database=hangfire;Username=postgres;Password=postgres",
    "PostgreSql": {
      "SchemaName": "hangfire",
      "PrepareSchemaIfNecessary": true
    },
    "Server": {
      "WorkerCount": 20
    }
  }
}
```

`PostgreSql` 节点对应 `Hangfire.PostgreSql.PostgreSqlStorageOptions`。

## 手动注册

```csharp
services.AddZStackHangirePostgreSqlStorage(options =>
{
    // 额外 Hangfire 配置
});
```
