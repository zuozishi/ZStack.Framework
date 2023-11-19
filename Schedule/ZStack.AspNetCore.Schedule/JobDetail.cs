using System.ComponentModel;

namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 系统作业信息
/// </summary>
public class JobDetail
{
    /// <summary>
    /// 作业Id
    /// </summary>
    public virtual string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 组名称
    /// </summary>
    public virtual string? GroupName { get; set; } = "default";

    /// <summary>
    /// 作业类型
    /// </summary>
    public virtual string? JobType { get; set; }

    /// <summary>
    /// 程序集
    /// </summary>
    public virtual string? AssemblyName { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 是否并行执行
    /// </summary>
    public virtual bool Concurrent { get; set; } = true;

    /// <summary>
    /// 是否扫描特性触发器
    /// </summary>
    public virtual bool IncludeAnnotations { get; set; } = false;

    /// <summary>
    /// 额外数据
    /// </summary>
    public virtual string? Properties { get; set; } = "{}";

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 作业创建类型
    /// </summary>
    public virtual JobCreateTypeEnum CreateType { get; set; } = JobCreateTypeEnum.BuiltIn;

    /// <summary>
    /// 脚本代码
    /// </summary>
    public virtual string? ScriptCode { get; set; }
}

/// <summary>
/// 作业创建类型枚举
/// </summary>
[Description("作业创建类型枚举")]
public enum JobCreateTypeEnum
{
    /// <summary>
    /// 内置
    /// </summary>
    [Description("内置")]
    BuiltIn = 0,

    /// <summary>
    /// 脚本
    /// </summary>
    [Description("脚本")]
    Script = 1,

    /// <summary>
    /// HTTP请求
    /// </summary>
    [Description("HTTP请求")]
    Http = 2,
}
