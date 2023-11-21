using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.EventBus;

public class MessageDispatcher(IServiceProvider _sp) : IAutoSubscriberMessageDispatcher
{
    void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
    {
        using var scope = _sp.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
        consumer.Consume(message, cancellationToken);
    }

    Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
    {
        using var scope = _sp.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
        return consumer.ConsumeAsync(message, cancellationToken);
    }
}
