namespace ZStack.QingTui;

public class UpdateMemberReq
{
    /// <summary>
    /// 企业内用户Id
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 所在组织机构
    /// </summary>
    public string? OrgId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string? EmployeeId { get; set; }
}
