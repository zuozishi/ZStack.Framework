namespace ZStack.QingTui;

public class CreateMemberReq
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 初始用户密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 所在组织机构
    /// </summary>
    public string OrgId { get; set; } = string.Empty;

    /// <summary>
    /// 电邮地址
    /// </summary>
    public string? Mail { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string? EmployeeId { get; set; }
}
