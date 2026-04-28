# ZStack.AspNetCore.EventBus

基于 EasyNetQ 的事件总线组件，提供自动订阅、消息分发和异常处理能力。

## 快速开始

添加 `ZStack.AspNetCore.EventBus` NuGet 包后，组件会自动发现并注册。配置：

```json
{
  "EventBus": {
    "Host": "rabbitmq.example.com",
    "Port": 5672,
    "UserName": "admin",
    "Password": "admin",
    "Prefix": "MyApp",           // 队列名前缀
    "ManagementUrl": "http://rabbitmq.example.com:15672"
  }
}
```

## 消息发布与订阅

### 定义消息

```csharp
[Queue("MyExchange", QueueName = "MyQueue")]
public class UserCreatedEvent
{
    public string UserId { get; set; }
    public string Name { get; set; }
}
```

### 发布消息

```csharp
var bus = App.GetRequiredService<IBus>();
await bus.PubSub.PublishAsync(new UserCreatedEvent { UserId = "1", Name = "张三" });
```

### 订阅消息（自动）

实现 `IConsume<T>` 或 `IConsumeAsync<T>` 即可被自动发现并订阅：

```csharp
public class UserCreatedHandler : IConsumeAsync<UserCreatedEvent>
{
    public async Task ConsumeAsync(UserCreatedEvent message, CancellationToken cancellationToken)
    {
        // 处理消息
    }
}
```

## 错误处理

```csharp
// 通过泛型注册错误处理器
services.AddEventBusErrorHandler<MyErrorHandler>();

// 通过委托注册
services.AddEventBusErrorHandler((error, ct) =>
{
    Console.WriteLine($"消息处理错误: {error.RoutingKey}");
    return Task.CompletedTask;
});
```

自定义错误处理器需实现 `IEventBusErrorHandler`：

```csharp
public class MyErrorHandler : IEventBusErrorHandler
{
    public Task OnError(Error error, CancellationToken token)
    {
        // 处理错误
        return Task.CompletedTask;
    }
}
```

## Queue 命名规则

队列命名格式：`{Prefix}:{Namespace}.{TypeName}:{ConsumerName}`

可通过 `[Queue]` 特性自定义交换器和队列名。
