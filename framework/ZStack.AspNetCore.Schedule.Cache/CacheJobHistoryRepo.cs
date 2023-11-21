using NewLife.Caching;
using ZStack.Core;

namespace ZStack.AspNetCore.Schedule.Cache;

public class CacheJobHistoryRepo(ICache _cache) : IJobHistoryRepo
{
    private readonly string _prefix = "Job:History:";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="jobHistory"></param>
    /// <returns></returns>
    public Task SaveAsync(JobHistory jobHistory)
    {
        string key = $"{_prefix}{jobHistory.JobId}|{jobHistory.RunId}";
        _cache.Add(key, jobHistory);
        return Task.CompletedTask;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="runId"></param>
    /// <returns></returns>
    public Task<JobHistory?> GetAsync(string jobId, Guid runId)
    {
        string key = $"{_prefix}{jobId}|{runId}";
        var jobHistory = _cache.Get<JobHistory>(key);
        return Task.FromResult(jobHistory);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="triggerId"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public Task<List<JobHistory>> GetPagedAsync(string jobId, string? triggerId, int pageIndex, int pageSize, RefAsync<int> total)
    {
        var keys = _cache.Search(_prefix + jobId + "|*").ToArray();
        if (keys.Length == 0)
        {
            total.Value = 0;
            return Task.FromResult(new List<JobHistory>());
        }
        var histories = _cache.GetAll<JobHistory>(keys)
            .Values
            .Where(x => x != null)
            .ToList()!;
        if (!string.IsNullOrEmpty(triggerId))
            histories = histories.Where(x => x!.TriggerId == triggerId).ToList();
        total.Value = histories.Count;
        return Task.FromResult((List<JobHistory>)histories
            .OrderByDescending(x => x!.OccurrenceTime)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList()!);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="runId"></param>
    /// <returns></returns>
    public Task RemoveAsync(string jobId, Guid runId)
    {
        string key = $"{_prefix}{jobId}|{runId}";
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="maxHistoryCount"></param>
    /// <param name="maxHistoryDays"></param>
    /// <returns></returns>
    public Task ClearAsync(int? maxHistoryCount, int? maxHistoryDays)
    {
        if (maxHistoryCount == null && maxHistoryDays == null)
            return Task.CompletedTask;
        var keys = _cache.Search(_prefix + "*").ToArray();
        var histories = _cache.GetAll<JobHistory>(keys)
            .Values
            .Where(x => x != null)
            .ToArray();
        var jobHistories = histories
            .GroupBy(x => x!.JobId)
            .ToList();
        foreach (var jobHistory in jobHistories)
        {
            var list = jobHistory.OrderByDescending(x => x!.OccurrenceTime).ToList();
            var removeKeys = new List<string>();
            if (maxHistoryCount != null)
                removeKeys.AddRange(list
                    .Skip(maxHistoryCount.Value)
                    .Select(x => $"{_prefix}{x!.JobId}|{x!.RunId}"));
            if (maxHistoryDays != null)
            {
                var maxHistoryTime = DateTime.Now.AddDays(-maxHistoryDays.Value);
                removeKeys.AddRange(list
                    .Where(x => x!.OccurrenceTime < maxHistoryTime)
                    .Select(x => $"{_prefix}{x!.JobId}|{x!.RunId}"));
            }
            _cache.Remove(removeKeys.Distinct().ToArray());
        }
        return Task.CompletedTask;
    }
}
