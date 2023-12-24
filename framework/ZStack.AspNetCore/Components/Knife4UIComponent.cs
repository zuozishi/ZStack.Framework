using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;

namespace ZStack.AspNetCore.Components;

public class Knife4UIComponent : IApplicationComponent
{
    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        app.UseKnife4UI();
    }
}
