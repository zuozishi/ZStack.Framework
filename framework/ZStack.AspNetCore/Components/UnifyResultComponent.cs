namespace ZStack.AspNetCore.Components;

/// <summary>
/// 统一结果组件
/// </summary>
[DependsOn(typeof(ApiControllerComponent))]
public class UnifyResultComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddUnifyResult();
    }
}
