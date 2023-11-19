using EasyNetQ.SystemMessages;

namespace ZStack.AspNetCore.EventBus;

internal class FuncEventBusErrorHandler(Func<Error, CancellationToken, Task> action) : IEventBusErrorHandler
{
    public async Task OnError(Error error, CancellationToken token)
    {
        await action.Invoke(error, token);
    }
}
