using Microsoft.AspNetCore.Builder;
using System.Reflection;
using System.Xml;

namespace ZStack.AspNetCore.Components;

public class SwashbuckleComponent : IServiceComponent, IApplicationComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        if (services is null)
            return;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // 装载 XML 文档
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            var doc = new XmlDocument();
            foreach (var xmlFile in xmlFiles)
            {
                doc.Load(xmlFile);
                var assemblyName = doc.SelectSingleNode("//doc/assembly/name")?.InnerText;
                if (assemblyName != null)
                {
                    options.IncludeXmlComments(xmlFile);
                }
            }
        });
    }

    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
