using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Management.Client;
using EasyNetQ.SystemMessages;
using ZStack.AspNetCore;
using ZStack.AspNetCore.EventBus;

namespace Microsoft.Extensions.DependencyInjection;

public static class EventBusSetup
{
    /// <summary>
    /// 注册EasyNetQ
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddZStackEventBus(this IServiceCollection service, Action<EventBusOptions>? configure = null)
    {
        var options = App.GetOptions<EventBusOptions>();
        service.AddConfigurableOptions<EventBusOptions>();
        service.AddSingleton<IConventions, QueueNamingConventions>();
        service.RegisterEasyNetQ(_ =>
        {
            configure?.Invoke(options);
            return options;
        });
        service.AddSingleton<MessageDispatcher>();
        service.AddSingleton(sp => new AutoSubscriber(sp.GetRequiredService<IBus>(), options.Prefix)
        {
            AutoSubscriberMessageDispatcher = sp.GetRequiredService<MessageDispatcher>(),
            GenerateSubscriptionId = c => $"{c.ConcreteType.Name}.{c.MessageType.Name}",
        });
        service.AddHostedService<AutoSubscriberService>();
        if (!string.IsNullOrEmpty(options.ManagementUrl))
        {
            var client = new ManagementClient(new Uri(options.ManagementUrl), options.UserName, options.Password);
            service.AddSingleton<IManagementClient>(client);
        }
        return service;
    }

    /// <summary>
    /// 注册异常事件处理器
    /// </summary>
    /// <typeparam name="IHandler"></typeparam>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusErrorHandler<IHandler>(this IServiceCollection service)
        where IHandler : class, IEventBusErrorHandler
    {
        service.AddScoped<IEventBusErrorHandler, IHandler>();
        service.AddHostedService<EventBusErrorReader>();
        return service;
    }

    /// <summary>
    /// 注册异常事件处理器
    /// </summary>
    /// <param name="service"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusErrorHandler(this IServiceCollection service, Func<Error, CancellationToken, Task> action)
    {
        service.AddScoped<IEventBusErrorHandler>(sp => new FuncEventBusErrorHandler(action));
        service.AddHostedService<EventBusErrorReader>();
        return service;
    }
}
