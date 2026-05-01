using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ZStack.AspNetCore.EventBus;

internal class AutoSubscriberService(IServiceProvider _sp, IOptions<EventBusOptions> _options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bus = _sp.GetRequiredService<IBus>();
        var subscriber = new AutoSubscriber(bus, _sp, _options.Value.Prefix)
        {
            AutoSubscriberMessageDispatcher = _sp.GetRequiredService<MessageDispatcher>(),
            GenerateSubscriptionId = c => $"{c.ConcreteType.Name}.{c.MessageType.Name}",
        };
        await subscriber.SubscribeAsync([.. App.EffectiveTypes], stoppingToken);
    }
}
