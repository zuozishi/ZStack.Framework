using EasyNetQ;
using Furion.ConfigurableOptions;

namespace ZStack.AspNetCore.EventBus;

public class EventBusOptions : ConnectionConfiguration, IConfigurableOptions
{
    public string? Prefix { get; set; }

    public string? ManagementUrl { get; set; }
}
