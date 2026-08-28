# ZStackWebApi

这是一个基于 ZStack.Framework 的 ASP.NET Core Web API 示例。

## 启动

```bash
dotnet run
```

- 健康检查：`GET /api/health`
- API 文档：`/scalar`

## 组件选择

通过 `dotnet new zstack-webapi` 创建项目时，可以重复传入 `--components` 组合组件：

```bash
dotnet new zstack-webapi -n ZStackWebApi \
  --components cache \
  --components hangfire \
  --components opentelemetry
```

生成的项目只包含被选中的组件包、配置和 `Examples/` 下的示例代码。

| 组件 | 示例 |
| --- | --- |
| `cache` | `POST /api/examples/cache` 写入，`GET /api/examples/cache` 读取 |
| `eventbus` | `POST /api/examples/eventbus` 发布 RabbitMQ 消息 |
| `hangfire` | `/hangfire` 查看任务面板，示例任务每五分钟执行 |
| `sqlsugar` | `GET /api/examples/sqlsugar` 查看数据源配置 |
| `opentelemetry` | `GET /api/examples/telemetry` 记录一次 Trace 和 Metric |
| `qingtui` | `GET /api/examples/qingtui` 查看轻推客户端配置 |

组件配置位于 `Configuration/`。EventBus 需要 RabbitMQ，Hangfire 的 Redis/PostgreSQL 存储需要填写连接字符串，QingTui 需要替换示例凭据。
