namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化模型特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UnifyModelAttribute(Type modelType) : Attribute
{
    /// <summary>
    /// 模型类型（泛型）
    /// </summary>
    public Type ModelType { get; set; } = modelType;
}
