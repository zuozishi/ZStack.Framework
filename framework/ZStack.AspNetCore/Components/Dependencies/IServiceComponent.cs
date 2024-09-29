namespace System;

/// <summary>
/// 服务组件
/// </summary>
public interface IServiceComponent : IComponent
{
    void Load(IServiceCollection services, ComponentContext componentContext);
}
