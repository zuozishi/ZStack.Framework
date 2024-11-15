namespace ZStack.AspNetCore.Components;

/// <summary>
/// 统一接口响应组件
/// </summary>
[DependsOn(typeof(ApiControllerComponent))]
public class UnifyResultComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddUnifyResult();
    }
}
