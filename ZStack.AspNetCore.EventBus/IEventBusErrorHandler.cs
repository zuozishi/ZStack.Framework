using EasyNetQ.SystemMessages;

namespace ZStack.AspNetCore.EventBus;

public interface IEventBusErrorHandler
{
    Task OnError(Error error, CancellationToken token);
}
