using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZStack.AspNetCore.EventBus;

internal class AutoSubscriberService(IServiceProvider _sp) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = _sp.GetRequiredService<AutoSubscriber>();
        await subscriber.SubscribeAsync(App.EffectiveTypes.ToArray(), stoppingToken);
    }
}
