# ZStack.AspNetCore.QingTui

轻推 API 客户端的 ASP.NET Core 组件封装，支持多应用配置和基于 Cache 的 Token 持久化。

## 快速开始

配置：

```json
{
  "QingTuiClient": {
    "Host": "https://open.qingtui.com",
    "Clients": [
      {
        "DisplayName": "我的应用",
        "AppId": "your_appid",
        "Secret": "your_secret"
      }
    ]
  }
}
```

`QingTuiClientComponent` 会自动发现并注册。然后获取客户端：

```csharp
var service = App.GetRequiredService<IQingTuiClientService>();
var client = service.Get("your_appid");

// 发送消息
var msgId = await client.SendTextMessageAsync("Hello");
```

## 多应用管理

```csharp
// 动态添加客户端
service.Add(new QingTuiApiClient(options));

// 获取指定应用客户端
var client = service.Get("another_appid");
```

## Token 持久化

`CacheTokenPersister` 使用框架的 `ICache`（支持内存和 Redis）存储 Token 和 JsTicket，多实例共享。

实现 `ITokenPersister` 可自定义持久化方式。
