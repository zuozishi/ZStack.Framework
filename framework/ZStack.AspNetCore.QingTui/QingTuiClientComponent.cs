using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.QingTui;

public class QingTuiClientComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddQingTuiClient();
    }
}
