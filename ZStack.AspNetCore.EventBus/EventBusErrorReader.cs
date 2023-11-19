using EasyNetQ;
using EasyNetQ.SystemMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ZStack.AspNetCore.EventBus;

public class EventBusErrorReader(IBus _bus, IServiceProvider _sp, IOptions<EventBusOptions> _options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<Error>(nameof(EventBusErrorReader), OnMessage, configure =>
        {
            configure.WithQueueName(_options.Value.Prefix + ":Default_Error_Queue");
        }, stoppingToken);
    }

    private async Task OnMessage(Error error, CancellationToken token)
    {
        using var scope = _sp.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IEventBusErrorHandler>();
        await handler.OnError(error, token);
    }
}
