namespace ZStack.SqlSugar;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SeedDataAttribute : Attribute
{
    /// <summary>
    /// 是否根据条件新增/更新
    /// </summary>
    public bool ByConditional { get; set; } = false;

    /// <summary>
    /// 是否更新
    /// </summary>
    public bool Update { get; set; } = false;
}
