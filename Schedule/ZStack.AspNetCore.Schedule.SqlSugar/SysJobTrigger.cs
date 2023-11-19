using Furion.Schedule;
using SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

[SugarTable(null, "系统作业触发器表")]
public class SysJobTrigger : JobTrigger
{
    [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
    public long Id { get; set; }

    [SugarColumn(ColumnDescription = "触发器Id", Length = 64)]
    public override string TriggerId { get => base.TriggerId; set => base.TriggerId = value; }

    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    public override string JobId { get => base.JobId; set => base.JobId = value; }

    [SugarColumn(ColumnDescription = "触发器类型", Length = 128)]
    public override string? TriggerType { get => base.TriggerType; set => base.TriggerType = value; }

    [SugarColumn(ColumnDescription = "程序集", Length = 128)]
    public override string? AssemblyName { get => base.AssemblyName; set => base.AssemblyName = value; }

    [SugarColumn(ColumnDescription = "参数", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? Args { get => base.Args; set => base.Args = value; }

    [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
    public override string? Description { get => base.Description; set => base.Description = value; }

    [SugarColumn(ColumnDescription = "状态")]
    public override TriggerStatus Status { get => base.Status; set => base.Status = value; }

    [SugarColumn(ColumnDescription = "起始时间")]
    public override DateTime? StartTime { get => base.StartTime; set => base.StartTime = value; }

    [SugarColumn(ColumnDescription = "结束时间")]
    public override DateTime? EndTime { get => base.EndTime; set => base.EndTime = value; }

    [SugarColumn(ColumnDescription = "最近运行时间")]
    public override DateTime? LastRunTime { get => base.LastRunTime; set => base.LastRunTime = value; }

    [SugarColumn(ColumnDescription = "下一次运行时间")]
    public override DateTime? NextRunTime { get => base.NextRunTime; set => base.NextRunTime = value; }

    [SugarColumn(ColumnDescription = "触发次数")]
    public override long NumberOfRuns { get => base.NumberOfRuns; set => base.NumberOfRuns = value; }

    [SugarColumn(ColumnDescription = "最大触发次数")]
    public override long MaxNumberOfRuns { get => base.MaxNumberOfRuns; set => base.MaxNumberOfRuns = value; }

    [SugarColumn(ColumnDescription = "出错次数")]
    public override long NumberOfErrors { get => base.NumberOfErrors; set => base.NumberOfErrors = value; }

    [SugarColumn(ColumnDescription = "最大出错次数")]
    public override long MaxNumberOfErrors { get => base.MaxNumberOfErrors; set => base.MaxNumberOfErrors = value; }

    [SugarColumn(ColumnDescription = "重试次数")]
    public override int NumRetries { get => base.NumRetries; set => base.NumRetries = value; }

    [SugarColumn(ColumnDescription = "重试间隔时间（ms）")]
    public override int RetryTimeout { get => base.RetryTimeout; set => base.RetryTimeout = value; }

    [SugarColumn(ColumnDescription = "是否立即启动")]
    public override bool StartNow { get => base.StartNow; set => base.StartNow = value; }

    [SugarColumn(ColumnDescription = "是否启动时执行一次")]
    public override bool RunOnStart { get => base.RunOnStart; set => base.RunOnStart = value; }

    [SugarColumn(ColumnDescription = "是否重置触发次数")]
    public override bool ResetOnlyOnce { get => base.ResetOnlyOnce; set => base.ResetOnlyOnce = value; }

    [SugarColumn(ColumnDescription = "更新时间")]
    public override DateTime? UpdatedTime { get => base.UpdatedTime; set => base.UpdatedTime = value; }
}
