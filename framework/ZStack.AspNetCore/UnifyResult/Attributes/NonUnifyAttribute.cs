namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 禁止规范化处理
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class NonUnifyAttribute : Attribute
{
}
