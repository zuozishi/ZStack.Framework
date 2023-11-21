using Furion.ConfigurableOptions;

namespace ZStack.AspNetCore.Schedule;

public class ScheduleOptions : IConfigurableOptions
{
    /// <summary>
    /// 保存作业日志
    /// </summary>
    public bool SaveJobLog { get; set; } = true;

    /// <summary>
    /// 最大作业历史记录数量
    /// </summary>
    public int? MaxHistoryCount { get; set; }

    /// <summary>
    /// 最大作业历史记录天数
    /// </summary>
    public int? MaxHistoryDays { get; set; }
}
