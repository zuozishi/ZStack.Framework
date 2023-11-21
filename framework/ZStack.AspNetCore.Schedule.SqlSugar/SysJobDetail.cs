using SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

[SugarTable(null, "系统作业信息表")]
public class SysJobDetail : JobDetail
{
    [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
    public long Id { get; set; }

    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    public override string JobId { get => base.JobId; set => base.JobId = value; }

    [SugarColumn(ColumnDescription = "组名称", Length = 128)]
    public override string? GroupName { get => base.GroupName; set => base.GroupName = value; }

    [SugarColumn(ColumnDescription = "作业类型", Length = 128)]
    public override string? JobType { get => base.JobType; set => base.JobType = value; }

    [SugarColumn(ColumnDescription = "程序集", Length = 128)]
    public override string? AssemblyName { get => base.AssemblyName; set => base.AssemblyName = value; }

    [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
    public override string? Description { get => base.Description; set => base.Description = value; }

    [SugarColumn(ColumnDescription = "是否并行执行")]
    public override bool Concurrent { get => base.Concurrent; set => base.Concurrent = value; }

    [SugarColumn(ColumnDescription = "是否扫描特性触发器")]
    public override bool IncludeAnnotations { get => base.IncludeAnnotations; set => base.IncludeAnnotations = value; }

    [SugarColumn(ColumnDescription = "额外数据", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? Properties { get => base.Properties; set => base.Properties = value; }

    [SugarColumn(ColumnDescription = "更新时间")]
    public override DateTime? UpdatedTime { get => base.UpdatedTime; set => base.UpdatedTime = value; }

    [SugarColumn(ColumnDescription = "作业创建类型")]
    public override JobCreateTypeEnum CreateType { get => base.CreateType; set => base.CreateType = value; }

    [SugarColumn(ColumnDescription = "脚本代码", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public override string? ScriptCode { get => base.ScriptCode; set => base.ScriptCode = value; }
}
