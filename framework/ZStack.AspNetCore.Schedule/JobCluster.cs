using Furion.Schedule;

namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 作业集群
/// </summary>
public class JobCluster
{
    /// <summary>
    /// 作业集群Id
    /// </summary>
    public virtual string ClusterId { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public virtual ClusterStatus Status { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdatedTime { get; set; }
}
