using Furion.Schedule;
using SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

[SugarTable(null, "系统作业集群表")]
public class SysJobCluster : JobCluster
{
    [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
    public long Id { get; set; }

    [SugarColumn(ColumnDescription = "作业集群Id", Length = 64)]
    public override string ClusterId { get => base.ClusterId; set => base.ClusterId = value; }

    [SugarColumn(ColumnDescription = "状态")]
    public override ClusterStatus Status { get => base.Status; set => base.Status = value; }

    [SugarColumn(ColumnDescription = "更新时间")]
    public override DateTime? UpdatedTime { get => base.UpdatedTime; set => base.UpdatedTime = value; }
}
