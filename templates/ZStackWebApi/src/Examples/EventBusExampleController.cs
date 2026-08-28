using EasyNetQ;
using EasyNetQ.AutoSubscribe;

namespace ZStackWebApi.Examples;

/// <summary>
/// EventBusComponent 示例消息。
/// </summary>
[Queue("ZStackTemplateEvents")]
public sealed class GreetingEvent
{
    public string Message { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// 自动订阅 GreetingEvent，并将消息写入日志。
/// </summary>
public sealed class GreetingEventHandler : IConsumeAsync<GreetingEvent>
{
    public Task ConsumeAsync(GreetingEvent message, CancellationToken cancellationToken)
    {
        App.Logger.LogInformation(
            "收到 EventBus 消息: {Message}, CreatedAt={CreatedAt}",
            message.Message,
            message.CreatedAt);
        return Task.CompletedTask;
    }
}

[ApiController]
[Route("api/examples/eventbus")]
public sealed class EventBusExampleController(IBus bus) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Publish(
        [FromBody] PublishGreetingRequest request,
        CancellationToken cancellationToken)
    {
        var message = new GreetingEvent
        {
            Message = string.IsNullOrWhiteSpace(request.Message)
                ? "hello from zstack eventbus"
                : request.Message,
            CreatedAt = DateTime.UtcNow
        };
        await bus.PubSub.PublishAsync(message, cancellationToken);
        return Accepted(new { message.Message, message.CreatedAt });
    }
}

public sealed record PublishGreetingRequest(string? Message);
