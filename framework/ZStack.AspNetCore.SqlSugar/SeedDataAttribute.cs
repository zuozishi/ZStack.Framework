namespace ZStack.AspNetCore.SqlSugar;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SeedDataAttribute(string? uniqueField = null, bool update = false) : Attribute
{
    /// <summary>
    /// 唯一字段
    /// </summary>
    public string? UniqueField { get; } = uniqueField;

    /// <summary>
    /// 是否更新
    /// </summary>
    public bool Update { get; } = update;
}
