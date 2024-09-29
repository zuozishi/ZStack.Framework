namespace System;

/// <summary>
/// 组件依赖配置特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// 依赖组件列表
    /// </summary>
    private Type[] _dependComponents = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dependComponents">依赖组件列表</param>
    /// <remarks>支持字符串类型程序集/类型配置</remarks>
    public DependsOnAttribute(params Type[] dependComponents)
    {
        DependComponents = dependComponents;
    }

    /// <summary>
    /// 依赖组件列表
    /// </summary>
    public Type[] DependComponents {
        get => _dependComponents;
        set {
            var components = value ?? [];

            // 检查类型是否实现 IComponent 接口
            foreach (var type in components)
            {
                if (!typeof(IComponent).IsAssignableFrom(type))
                {
                    throw new InvalidOperationException($"The type of `{type.FullName}` must be assignable from `{nameof(IComponent)}`.");
                }
            }

            _dependComponents = components;
        }
    }
}
