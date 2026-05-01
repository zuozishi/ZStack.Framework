using EasyNetQ;
using EasyNetQ.Internals;
using Microsoft.Extensions.Options;

namespace ZStack.AspNetCore.EventBus;

public class QueueNamingConventions : Conventions
{
    public QueueNamingConventions(ITypeNameSerializer typeNameSerializer, IOptions<EventBusOptions> options) : base(typeNameSerializer)
    {
        string prefix = options.Value.Prefix ?? "default";
        ExchangeNamingConvention = type =>
        {
            var attr = GetQueueAttribute(type);

            return string.IsNullOrEmpty(attr.Name)
                ? $"{prefix}:{type.Namespace}.{type.Name}"
                : attr.Name;
        };

        QueueTypeConvention = type =>
        {
            var attr = GetQueueAttribute(type);

            return string.IsNullOrEmpty(attr.Type)
                ? null
                : attr.Type;
        };

        TopicNamingConvention = _ => "";

        QueueNamingConvention = (type, subscriptionId) =>
        {
            var attr = GetQueueAttribute(type);

            if (string.IsNullOrEmpty(attr.Name))
            {
                var typeName = typeNameSerializer.Serialize(type);

                return string.IsNullOrEmpty(subscriptionId)
                    ? typeName
                    : $"{prefix}:{type.Namespace}.{type.Name}:{subscriptionId.Split('.').First()}";
            }

            return string.IsNullOrEmpty(subscriptionId)
                ? attr.Name
                : $"{attr.Name}_{subscriptionId}";
        };
        RpcRoutingKeyNamingConvention = typeNameSerializer.Serialize;

        ErrorQueueNamingConvention = _ => $"{prefix}:Default_Error_Queue";
        ErrorExchangeNamingConvention = receivedInfo => $"{prefix}:ErrorExchange_{receivedInfo.RoutingKey}";
        RpcRequestExchangeNamingConvention = _ => "easy_net_q_rpc";
        RpcResponseExchangeNamingConvention = _ => "easy_net_q_rpc";
        RpcReturnQueueNamingConvention = _ => "easynetq.response." + Guid.NewGuid();

        ConsumerTagConvention = () => Guid.NewGuid().ToString();
    }

    private static QueueAttribute GetQueueAttribute(Type messageType)
        => messageType.GetAttribute<QueueAttribute>() ?? new(string.Empty);
}
