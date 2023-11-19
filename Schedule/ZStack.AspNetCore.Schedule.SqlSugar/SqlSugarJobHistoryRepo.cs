using Mapster;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using ZStack.AspNetCore.SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

public class SqlSugarJobHistoryRepo : IJobHistoryRepo
{
    private readonly SimpleClient<SysJobHistory> _repo;

    public SqlSugarJobHistoryRepo(IConfiguration configuration, ISqlSugarService sqlSugarService)
    {
        var configId = configuration.Get<string?>("Schedule:SqlSugarConfigId");
        var db = sqlSugarService.Get(configId);
        db.CodeFirst.SetStringDefaultLength(128)
            .InitTables<SysJobHistory>();
        _repo = db.GetSimpleClient<SysJobHistory>();
    }

    public async Task SaveAsync(JobHistory jobHistory)
    {
        var entity = jobHistory.Adapt<SysJobHistory>();
        await _repo.CopyNew().InsertAsync(entity);
    }

    public async Task<JobHistory?> GetAsync(string jobId, Guid runId)
    {
        return await _repo.GetFirstAsync(o => o.JobId == jobId && o.RunId == runId);
    }

    public async Task<List<JobHistory>> GetPagedAsync(string jobId, string? triggerId, int pageIndex, int pageSize, Core.RefAsync<int> total)
    {
        RefAsync<int> total1 = new();
        var list = await _repo.AsQueryable()
            .Where(o => o.JobId == jobId)
            .WhereIF(triggerId != null, o => o.TriggerId == triggerId)
            .ToPageListAsync(pageIndex, pageSize, total1);
        total.Value = total1.Value;
        return list.Adapt<List<JobHistory>>();
    }

    public async Task RemoveAsync(string jobId, Guid runId)
    {
        await _repo.DeleteAsync(o => o.JobId == jobId && o.RunId == runId);
    }

    public async Task ClearAsync(int? maxHistoryCount, int? maxHistoryDays)
    {
        if (maxHistoryCount == null && maxHistoryDays == null)
            return;
        var histories = await _repo.AsQueryable()
            .Select(o => new SysJobHistory
            {
                Id = o.Id,
                JobId = o.JobId,
                RunId = o.RunId,
                OccurrenceTime = o.OccurrenceTime,
            })
            .ToListAsync();
        var jobGroup = histories.GroupBy(o => o.JobId);
        foreach (var job in jobGroup)
        {
            var list = job.OrderByDescending(o => o.OccurrenceTime).ToList();
            var removeKeys = new List<dynamic>();
            if (maxHistoryCount != null)
            {
                removeKeys.AddRange(list
                    .Skip(maxHistoryCount.Value)
                    .Select(o => (dynamic)o.Id));
            }
            if (maxHistoryDays != null)
            {
                var time = DateTime.Now.AddDays(-maxHistoryDays.Value);
                removeKeys.AddRange(list
                    .Where(o => o.OccurrenceTime < time)
                    .Select(o => (dynamic)o.Id));
            }
            if (removeKeys.Count > 0)
                await _repo.DeleteByIdsAsync([.. removeKeys]);
        }
    }
}
