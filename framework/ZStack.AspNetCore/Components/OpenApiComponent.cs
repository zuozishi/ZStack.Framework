using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Scalar.AspNetCore;

namespace ZStack.AspNetCore.Components;

/// <summary>
/// API文档组件
/// </summary>
public class OpenApiComponent : IServiceComponent, IApplicationComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        if (services is null)
            return;
        services.AddZStackOptions<OpenApiOptions>();
        var options = App.GetOptions<OpenApiOptions>();
        if (!options.Enable)
            return;
        var groups = options.Groups;
        if (!groups.ContainsKey("default"))
            groups["default"] = "默认";
        foreach (var group in groups)
        {
            services.AddOpenApi(group.Key, c =>
            {
                c.AddDocumentTransformer((document, context, cancellationToken)
                    =>
                {
                    document.Info.Title = group.Value;
                    return Task.CompletedTask;
                });
            });
        }
    }

    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        var options = App.GetOptions<OpenApiOptions>();
        if (!options.Enable)
            return;
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapOpenApi();
            if (options.EnableScalar)
            {
                endpoints.MapScalarApiReference();
                endpoints.MapGet("/scalar", () => Results.Redirect("/scalar/default")).ExcludeFromDescription();
            }
        });
    }
}
