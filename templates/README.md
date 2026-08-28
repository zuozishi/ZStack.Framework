# ZStack.Framework 模板

`templates` 目录包含两个可发布为 NuGet template package 的脚手架：

- `zstack-console`：基于 `ZStack.Core` 的控制台应用。
- `zstack-webapi`：基于 `ZStack.AspNetCore` 的 ASP.NET Core Web API，可按需组合组件。

## 本地安装

```bash
dotnet new install ./templates/ConsoleApp/src
dotnet new install ./templates/ZStackWebApi/src
```

## 创建 Web API

不选择可选组件时生成最小项目：

```bash
dotnet new zstack-webapi -n Demo.Api
```

通过重复传入 `--components` 组合组件：

```bash
dotnet new zstack-webapi -n Demo.Api \
  --components cache \
  --components hangfire \
  --components opentelemetry
```

可选组件如下：

| 选项 | 示例 | 依赖/说明 |
| --- | --- | --- |
| `cache` | `/api/examples/cache` | 内存缓存，可改为 Redis 配置 |
| `eventbus` | `POST /api/examples/eventbus` | 需要 RabbitMQ |
| `hangfire` | `/hangfire` | 默认内存存储；可选 `memory`、`redis`、`postgresql` |
| `sqlsugar` | `/api/examples/sqlsugar` | 默认提供 SQLite 配置示例 |
| `opentelemetry` | `/api/examples/telemetry` | 支持 OTLP 环境变量导出 |
| `qingtui` | `/api/examples/qingtui` | 需要填写轻推 AppId/Secret |

Hangfire 使用其他存储时：

```bash
dotnet new zstack-webapi -n Demo.Api \
  --components hangfire \
  --hangfireStorage redis
```

模板会同时裁剪对应的 PackageReference、配置文件和示例文件。生成后执行：

```bash
dotnet run --project Demo.Api
```

基础接口为 `/api/health`，API 文档为 `/scalar`。

## 打包和验证

```bash
dotnet restore ./templates/ZStack.Framework.Templates.sln
dotnet build ./templates/ZStack.Framework.Templates.sln --configuration Release --no-restore
dotnet pack ./templates/ZStack.Framework.Templates.sln --configuration Release --no-build --output ./templates/nupkgs
pwsh ./templates/validate-templates.ps1
```
