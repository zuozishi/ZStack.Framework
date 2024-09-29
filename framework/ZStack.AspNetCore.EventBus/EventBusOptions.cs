using EasyNetQ;

namespace ZStack.AspNetCore.EventBus;

public class EventBusOptions : ConnectionConfiguration
{
    public string? Prefix { get; set; }

    public string? ManagementUrl { get; set; }
}
