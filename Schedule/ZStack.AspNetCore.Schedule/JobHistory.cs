namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 作业历史记录
/// </summary>
public class JobHistory
{
    /// <summary>
    /// 作业 Id
    /// </summary>
    public virtual string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 作业触发器 Id
    /// </summary>
    public virtual string TriggerId { get; set; } = string.Empty;

    /// <summary>
    /// 当前作业触发器触发的唯一标识
    /// </summary>
    public virtual Guid RunId { get; set; }

    // <summary>
    /// 作业计划触发时间
    /// </summary>
    public virtual DateTime OccurrenceTime { get; set; }

    /// <summary>
    /// 执行后时间
    /// </summary>
    public virtual DateTime ExecutedTime { get; set; }

    /// <summary>
    /// 本次执行结果
    /// </summary>
    public virtual string? Result { get; set; }

    /// <summary>
    /// 异常类型
    /// </summary>
    public virtual string? ExceptionType { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public virtual string? ExceptionMessage { get; set; }

    /// <summary>
    /// 异常栈信息
    /// </summary>
    public virtual string? ExceptionStackTrace { get; set; }
}
