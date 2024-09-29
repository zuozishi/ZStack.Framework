using Microsoft.Extensions.Hosting;

namespace System;

/// <summary>
/// Web组件
/// </summary>
public interface IWebComponent : IComponent
{
    void Load(IHostApplicationBuilder builder, ComponentContext componentContext);
}
