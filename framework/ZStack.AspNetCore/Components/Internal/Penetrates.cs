using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ZStack.AspNetCore.Components;

internal class Penetrates
{
    /// <summary>
    /// 添加主机组件
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void AddWebComponent(IHostApplicationBuilder builder, Type componentType, object? options = null)
    {
        if (!componentType.IsAssignableFrom(typeof(IWebComponent)))
            throw new InvalidOperationException("组件类型必须实现 IWebComponent 接口!");
        if (componentType.IsAbstract || componentType.IsInterface)
            throw new InvalidOperationException("组件类型不能是抽象类或接口!");
        // 创建组件上下文
        var componentContext = AddComponent(componentType, options);
        // 创建组件实例
        var component = Activator.CreateInstance(componentContext.ComponentType) as IWebComponent;
        // 调用
        component!.Load(builder, componentContext);
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void AddServiceComponent(IServiceCollection services, Type componentType, object? options = null)
    {
        if (!componentType.IsAssignableFrom(typeof(IServiceComponent)))
            throw new InvalidOperationException("组件类型必须实现 IServiceComponent 接口!");
        if (componentType.IsAbstract || componentType.IsInterface)
            throw new InvalidOperationException("组件类型不能是抽象类或接口!");
        // 创建组件上下文
        var componentContext = AddComponent(componentType, options);
        // 创建组件实例
        var component = Activator.CreateInstance(componentContext.ComponentType) as IServiceComponent;
        // 调用
        component!.Load(services, componentContext);
    }

    /// <summary>
    /// 添加中间件组件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    internal static void AddApplicationComponent(IApplicationBuilder app, Type componentType, object? options = null)
    {
        if (!componentType.IsAssignableFrom(typeof(IApplicationComponent)))
            throw new InvalidOperationException("组件类型必须实现 IApplicationComponent 接口!");
        if (componentType.IsAbstract || componentType.IsInterface)
            throw new InvalidOperationException("组件类型不能是抽象类或接口!");
        // 创建组件上下文
        var componentContext = AddComponent(componentType, options);
        // 创建组件实例
        var component = Activator.CreateInstance(componentContext.ComponentType) as IApplicationComponent;
        // 调用
        var loadMethods = componentContext.ComponentType.GetMethods();
        foreach (var method in loadMethods)
        {
            if (method.Name != "Load" && method.Name != "LoadAsync")
                continue;
            var args = new List<object?>();
            foreach (var parameter in method.GetParameters())
            {
                if (parameter.ParameterType == typeof(IApplicationBuilder))
                    args.Add(app);
                else if (parameter.ParameterType == typeof(ComponentContext))
                    args.Add(componentContext);
                else
                    args.Add(app.ApplicationServices.GetService(parameter.ParameterType));
            }
            var obj = method.Invoke(component, [.. args]);
            if (obj is Task task)
                task.Wait();
            else if (obj is ValueTask valueTask)
                valueTask.AsTask().Wait();
        }
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static ComponentContext AddComponent(Type componentType, object? options = null, ComponentContext? calledContext = null)
    {
        if (InternalApp.Components.Any(x => x.ComponentType == componentType))
            return InternalApp.Components.First(x => x.ComponentType == componentType);

        // 根组件上下文
        var componentContext = new ComponentContext(componentType)
        {
            IsRoot = calledContext == null,
            CalledContext = calledContext
        };
        if (options != null)
            componentContext.SetProperty(componentType, options);
        // 初始化组件依赖链

        // 获取 [DependsOn] 特性
        var dependsOnAttribute = componentType.GetCustomAttribute<DependsOnAttribute>(true);

        // 获取依赖组件列表
        var dependComponents = dependsOnAttribute?.DependComponents?.Distinct()?.ToArray() ?? [];

        // 检查自引用
        if (dependComponents.Contains(componentType))
            throw new InvalidOperationException("组件不能依赖自身!");
        if (calledContext != null && dependComponents.Contains(calledContext.ComponentType))
            throw new InvalidOperationException("组件不能依赖调用它的组件!");

        componentContext.DependComponents = dependComponents;

        // 遍历当前组件依赖的组件集合
        foreach (var dependComponent in dependComponents)
        {
            AddComponent(dependComponent, options, componentContext);
        }

        // 添加到组件列表
        InternalApp.Components.Add(componentContext);

        return componentContext;
    }
}
