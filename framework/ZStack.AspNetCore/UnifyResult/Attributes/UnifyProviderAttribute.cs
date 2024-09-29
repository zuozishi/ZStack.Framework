namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化提供器特性
/// </summary>
/// <param name="name"></param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class UnifyProviderAttribute(string name) : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UnifyProviderAttribute()
        : this(string.Empty)
    {
    }

    /// <summary>
    /// 提供器名称
    /// </summary>
    public string Name { get; set; } = name;
}
