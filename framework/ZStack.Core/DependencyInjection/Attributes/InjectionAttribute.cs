namespace ZStack.Core.DependencyInjection;

/// <summary>
/// 设置依赖注入方式
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="action"></param>
/// <param name="exceptInterfaces"></param>
[AttributeUsage(AttributeTargets.Class)]
public class InjectionAttribute(InjectionActions action, params Type[] exceptInterfaces) : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="exceptInterfaces"></param>
    public InjectionAttribute(params Type[] exceptInterfaces)
        : this(InjectionActions.Add, exceptInterfaces)
    {
    }

    /// <summary>
    /// 添加服务方式，存在不添加，或继续添加
    /// </summary>
    public InjectionActions Action { get; set; } = action;

    /// <summary>
    /// 注册选项
    /// </summary>
    public InjectionPatterns Pattern { get; set; } = InjectionPatterns.SelfWithFirstInterface;

    /// <summary>
    /// 注册别名
    /// </summary>
    /// <remarks>多服务时使用</remarks>
    public object? Key { get; set; }

    /// <summary>
    /// 排序，排序越大，则在后面注册
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// 排除接口
    /// </summary>
    public Type[] ExceptInterfaces { get; set; } = exceptInterfaces ?? [];

    /// <summary>
    /// 代理类型，必须继承 DispatchProxy、IDispatchProxy
    /// </summary>
    public Type? Proxy { get; set; }
}
