namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化元数据
/// </summary>
internal sealed class UnifyMetadata(string providerName, Type providerType, Type resultType)
{
    /// <summary>
    /// 提供器名称
    /// </summary>
    public string ProviderName { get; } = providerName;

    /// <summary>
    /// 提供器类型
    /// </summary>
    public Type ProviderType { get; } = providerType;

    /// <summary>
    /// 统一的结果类型
    /// </summary>
    public Type ResultType { get; } = resultType;
}
