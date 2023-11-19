using SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

[SugarTable(null, "系统作业执行历史表")]
public class SysJobHistory : JobHistory
{
    [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
    public long Id { get; set; }

    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    public override string JobId { get => base.JobId; set => base.JobId = value; }

    [SugarColumn(ColumnDescription = "作业触发器Id", Length = 64)]
    public override string TriggerId { get => base.TriggerId; set => base.TriggerId = value; }

    [SugarColumn(ColumnDescription = "当前作业触发器触发的唯一标识")]
    public override Guid RunId { get => base.RunId; set => base.RunId = value; }

    [SugarColumn(ColumnDescription = "作业计划触发时间")]
    public override DateTime OccurrenceTime { get => base.OccurrenceTime; set => base.OccurrenceTime = value; }

    [SugarColumn(ColumnDescription = "执行后时间")]
    public override DateTime ExecutedTime { get => base.ExecutedTime; set => base.ExecutedTime = value; }

    [SugarColumn(ColumnDescription = "本次执行结果", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? Result { get => base.Result; set => base.Result = value; }

    [SugarColumn(ColumnDescription = "异常类型", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? ExceptionType { get => base.ExceptionType; set => base.ExceptionType = value; }

    [SugarColumn(ColumnDescription = "异常信息", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? ExceptionMessage { get => base.ExceptionMessage; set => base.ExceptionMessage = value; }

    [SugarColumn(ColumnDescription = "异常栈信息", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? ExceptionStackTrace { get => base.ExceptionStackTrace; set => base.ExceptionStackTrace = value; }
}
