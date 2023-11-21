using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Hangfire.PostgreSql;

public class HangfirePostgreSqlComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackHangirePostgreSqlStorage();
    }
}
