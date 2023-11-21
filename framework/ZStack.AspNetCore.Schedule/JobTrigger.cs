using Furion.Schedule;

namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 系统作业触发器
/// </summary>
public class JobTrigger
{
    /// <summary>
    /// 触发器Id
    /// </summary>
    public virtual string TriggerId { get; set; } = string.Empty;

    /// <summary>
    /// 作业Id
    /// </summary>
    public virtual string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型
    /// </summary>
    public virtual string? TriggerType { get; set; }

    /// <summary>
    /// 程序集
    /// </summary>
    public virtual string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 参数
    /// </summary>
    public virtual string? Args { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public virtual TriggerStatus Status { get; set; } = TriggerStatus.Ready;

    /// <summary>
    /// 起始时间
    /// </summary>
    public virtual DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public virtual DateTime? EndTime { get; set; }

    /// <summary>
    /// 最近运行时间
    /// </summary>
    public virtual DateTime? LastRunTime { get; set; }

    /// <summary>
    /// 下一次运行时间
    /// </summary>
    public virtual DateTime? NextRunTime { get; set; }

    /// <summary>
    /// 触发次数
    /// </summary>
    public virtual long NumberOfRuns { get; set; }

    /// <summary>
    /// 最大触发次数
    /// </summary>
    public virtual long MaxNumberOfRuns { get; set; }

    /// <summary>
    /// 出错次数
    /// </summary>
    public virtual long NumberOfErrors { get; set; }

    /// <summary>
    /// 最大出错次数
    /// </summary>
    public virtual long MaxNumberOfErrors { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    public virtual int NumRetries { get; set; }

    /// <summary>
    /// 重试间隔时间（ms）
    /// </summary>
    public virtual int RetryTimeout { get; set; } = 1000;

    /// <summary>
    /// 是否立即启动
    /// </summary>
    public virtual bool StartNow { get; set; } = true;

    /// <summary>
    /// 是否启动时执行一次
    /// </summary>
    public virtual bool RunOnStart { get; set; } = false;

    /// <summary>
    /// 是否重置触发次数
    /// </summary>
    public virtual bool ResetOnlyOnce { get; set; } = true;

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdatedTime { get; set; }
}
