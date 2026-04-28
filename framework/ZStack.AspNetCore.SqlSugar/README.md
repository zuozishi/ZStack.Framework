# ZStack.AspNetCore.SqlSugar

ZStack.SqlSugar 的 ASP.NET Core 组件封装，自动集成雪花ID生成和数据库初始化。

## 快速开始

添加包后，`SqlSugarComponent` 会自动发现并注册（依赖 `CacheComponent`）。

确保 `ZStack.SqlSugar` 的配置已就绪（`DbConnection` 和 `SnowId` 配置节）。

## 分布式 ID 生成器

`IdGeneratorWorker` 是一个 `BackgroundService`，基于 Redis 锁自动分配 WorkerId（1-9），每5秒续约。

仅在 `Cache.CacheType` 为 `Redis` 时生效。

## 手动注册

```csharp
// 添加 SqlSugar + 雪花ID生成器
services.AddZStackSqlSugarWithSnowIdGenerator();

// 仅添加雪花ID生成器
services.AddSnowIdGenerator();
```
