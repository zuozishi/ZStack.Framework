using System.Reflection;
using ZStack.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 中间件注入（带Swagger）
    /// </summary>
    /// <param name="app"></param>
    /// <param name="autoLoadComponents">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackInject(
        this IApplicationBuilder app,
        bool autoLoadComponents = true,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        InternalApp.ServiceProvider = app.ApplicationServices;
        app.UseComponents(autoLoadComponents, components, ignoreComponents);

        if (InternalApp.HostEnvironment?.IsDevelopment() ?? false)
            app.ShowAppInfo();

        return app;
    }

    /// <summary>
    /// 自动注册中间件组件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="autoLoadComponents">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseComponents(this IApplicationBuilder app, bool autoLoadComponents = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        if (autoLoadComponents)
            App.EffectiveTypes
                .Where(t => (typeof(IApplicationComponent).IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract)
                .ForEach((type, _) =>
                {
                    if (componentList.Contains(type)) return;
                    if (ignoreComponents != null && ignoreComponents.Contains(type)) return;
                    componentList.Add(type);
                });
        componentList.ForEach((type, _) =>
        {
            App.Logger.LogInformation("注册中间件组件: {Component}", type);
            app.UseComponent(type);
        });
        return app;
    }

    /// <summary>
    /// 调用自定义Startups处理程序
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseStartups(this IApplicationBuilder app)
    {
        // 反转，处理排序
        var startups = InternalApp.AppStartups.Reverse();
        if (!startups.Any()) return app;

        // 遍历所有
        foreach (var startup in startups)
        {
            var type = startup.GetType();

            // 获取所有符合依赖注入格式的方法，如返回值 void 或 Task，且第一个参数是 IApplicationBuilder 类型
            var configureMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(u => (u.ReturnType == typeof(void) || u.ReturnType == typeof(Task))
                    && u.GetParameters().Length > 0
                    && u.GetParameters().First().ParameterType == typeof(IApplicationBuilder));

            if (!configureMethods.Any()) continue;

            // 自动安装属性调用
            foreach (var method in configureMethods)
            {
                var returnObj = method.Invoke(startup, ResolveMethodParameterInstances(app, method));
                if (returnObj is Task task)
                    task.Wait();
            }
        }

        // 释放内存
        InternalApp.AppStartups.Clear();
        return app;
    }

    /// <summary>
    /// 解析方法参数实例
    /// </summary>
    /// <param name="app"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    private static object[] ResolveMethodParameterInstances(IApplicationBuilder app, MethodInfo method)
    {
        // 获取方法所有参数
        var parameters = method.GetParameters();
        var parameterInstances = new object[parameters.Length];
        parameterInstances[0] = app;

        // 解析服务
        for (var i = 1; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            parameterInstances[i] = app.ApplicationServices.GetRequiredService(parameter.ParameterType);
        }

        return parameterInstances;
    }
}
