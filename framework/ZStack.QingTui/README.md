# ZStack.QingTui

轻推（QingTui）开放平台 API 客户端基础库，提供 Token 管理、JS-SDK 签名和 API 调用能力。

## 快速开始

```csharp
var options = QingTuiApiClientOptionsBuilder.Create()
    .SetAppId("your_appid")
    .SetSecret("your_secret")
    .SetDisplayName("我的应用")
    .SetHost("https://open.qingtui.com")
    .SetLogger(logger)
    .Build();

var client = new QingTuiApiClient(options);
```

## Token 管理

客户端会自动管理 Token 的获取和缓存。自定义 Token 持久化可实现 `ITokenPersister`：

```csharp
public interface ITokenPersister
{
    string? GetToken(string appId);
    void SaveToken(string appId, string token, int expire);
    string? GetJsTicket(string appId);
    void SaveJsTicket(string appId, string ticket, int expire);
}
```

默认使用 `LocalTokenPersister`（内存缓存）。

## API 调用

### 消息推送

```csharp
// 群发文字消息
var msgId = await client.SendTextMessageAsync("消息内容");

// 单发文字消息
var msgId = await client.SendTextMessageAsync("消息内容", "openId");

// 群发图文消息
var msgId = await client.SendNewsMessageAsync(articles);

// 群发图片消息
var msgId = await client.SendImageMessageAsync(mediaId);

// 群发 Markdown 消息
var msgId = await client.SendMarkdownMessageAsync("## 标题\n内容");
```

### 通讯录管理

```csharp
// 获取企业ID
var domainId = await client.GetDomainIdAsync("企业号");

// 创建组织机构
var orgId = await client.CreateOrgAsync("root", "部门名称");

// 更新组织机构
await client.UpdateOrgAsync(orgId, "新名称");

// 删除组织机构
await client.DeleteOrgAsync(orgId);
```

### JS-SDK 签名

```csharp
var signature = await client.GetJsSignatureAsync("https://example.com/page");
// signature.AppId, .Timestamp, .NonceStr, .Signature
```

更多 API 请参考源码中各 partial class 文件：
- `QingTuiApiClient.Message.cs` — 消息
- `QingTuiApiClient.Media.cs` — 素材
- `QingTuiApiClient.Org.cs` — 组织架构
- `QingTuiApiClient.Member.cs` — 成员管理
