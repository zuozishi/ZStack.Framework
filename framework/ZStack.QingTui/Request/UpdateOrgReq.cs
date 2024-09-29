namespace ZStack.QingTui;

public class UpdateOrgReq
{
    /// <summary>
    /// 组织机构Id
    /// </summary>
    public string OrgId { get; set; } = string.Empty;

    /// <summary>
    /// 组织机构名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 顺序值，范围是0~ 2147483647
    /// </summary>
    public int? Sequence { get; set; }

    /// <summary>
    /// 是否自动排序，默认为false不开启，如果开启此值，原组织中若已有待插入的sequence值，则会将原有值进行变更，以避免sequence出现重复
    /// </summary>
    public bool? AutoSequence { get; set; }
}
