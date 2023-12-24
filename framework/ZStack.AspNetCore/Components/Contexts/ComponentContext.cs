namespace System;

/// <summary>
/// 组件上下文
/// </summary>
public class ComponentContext(Type componentType)
{
    /// <summary>
    /// 组件类型
    /// </summary>
    public Type ComponentType { get; internal set; } = componentType;

    /// <summary>
    /// 上级组件上下文
    /// </summary>
    public ComponentContext? CalledContext { get; internal set; }

    /// <summary>
    /// 根组件上下文
    /// </summary>
    public ComponentContext? RootContext { get; internal set; }

    /// <summary>
    /// 依赖组件列表
    /// </summary>
    public Type[] DependComponents { get; internal set; } = [];

    /// <summary>
    /// 上下文数据
    /// </summary>
    private Dictionary<string, object> Properties { get; set; } = [];

    /// <summary>
    /// 是否是根组件
    /// </summary>
    internal bool IsRoot { get; set; }

    /// <summary>
    /// 设置组件属性参数
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public Dictionary<string, object> SetProperty<TComponent>(object value) where TComponent : class, IComponent, new()
    {
        return SetProperty(typeof(TComponent), value);
    }

    /// <summary>
    /// 设置组件属性参数
    /// </summary>
    /// <param name="componentType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Dictionary<string, object> SetProperty(Type componentType, object value)
    {
        return SetProperty(componentType.FullName!, value);
    }

    /// <summary>
    /// 设置组件属性参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Dictionary<string, object> SetProperty(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        Dictionary<string, object> dictionary = ((RootContext == null) ? Properties : RootContext.Properties);
        if (!dictionary.TryAdd(key, value))
        {
            dictionary[key] = value;
        }

        return dictionary;
    }

    /// <summary>
    /// 获取组件属性参数
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TComponentOptions"></typeparam>
    /// <returns></returns>
    public TComponentOptions? GetProperty<TComponent, TComponentOptions>() where TComponent : class, IComponent, new()
    {
        return GetProperty<TComponentOptions>(typeof(TComponent));
    }

    /// <summary>
    /// 获取组件属性参数
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    /// <param name="componentType"></param>
    /// <returns></returns>
    public TComponentOptions? GetProperty<TComponentOptions>(Type componentType)
    {
        return GetProperty<TComponentOptions>(componentType.FullName!);
    }

    /// <summary>
    /// 获取组件属性参数
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public TComponentOptions? GetProperty<TComponentOptions>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        Dictionary<string, object> dictionary = ((RootContext == null) ? Properties : RootContext.Properties);
        if (dictionary.TryGetValue(key, out object? value))
        {
            return (TComponentOptions)value;
        }

        return default;
    }

    /// <summary>
    /// 获取组件所有依赖参数
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object> GetProperties()
    {
        if (RootContext != null)
        {
            return RootContext.Properties;
        }

        return Properties;
    }
}
