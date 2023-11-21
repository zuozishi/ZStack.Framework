using ZStack.Core;

namespace ZStack.AspNetCore.Schedule;

public interface IJobHistoryRepo
{
    /// <summary>
    /// 保存作业历史记录
    /// </summary>
    /// <param name="jobHistory"></param>
    /// <returns></returns>
    Task SaveAsync(JobHistory jobHistory);

    /// <summary>
    /// 获取作业历史记录
    /// </summary>
    /// <param name="runId"></param>
    /// <returns></returns>
    Task<JobHistory?> GetAsync(string jobId, Guid runId);

    /// <summary>
    /// 分页获取作业历史记录
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="triggerId"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    Task<List<JobHistory>> GetPagedAsync(string jobId, string? triggerId, int pageIndex, int pageSize, RefAsync<int> total);

    /// <summary>
    /// 删除作业历史记录
    /// </summary>
    /// <param name="runId"></param>
    /// <returns></returns>
    Task RemoveAsync(string jobId, Guid runId);

    /// <summary>
    /// 清理过期作业历史记录
    /// </summary>
    /// <param name="maxHistoryCount"></param>
    /// <param name="maxHistoryDays"></param>
    /// <returns></returns>
    Task ClearAsync(int? maxHistoryCount, int? maxHistoryDays);
}
