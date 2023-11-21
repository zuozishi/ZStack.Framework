using Mapster;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using ZStack.AspNetCore.SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

public class SqlSugarJobDetailRepo : IJobDetailRepo
{
    private readonly SimpleClient<SysJobDetail> _jobDetailRepo;
    private readonly SimpleClient<SysJobTrigger> _triggerRepo;

    public SqlSugarJobDetailRepo(IConfiguration configuration, ISqlSugarService sqlSugarService)
    {
        var configId = configuration.Get<string?>("Schedule:SqlSugarConfigId");
        var db = sqlSugarService.Get(configId);
        db.CodeFirst.SetStringDefaultLength(128)
            .InitTables<SysJobDetail, SysJobTrigger>();
        _jobDetailRepo = db.GetSimpleClient<SysJobDetail>();
        _triggerRepo = db.GetSimpleClient<SysJobTrigger>();
    }

    public List<JobDetail> GetJobs()
    {
        var list = _jobDetailRepo.CopyNew().GetList();
        return list.Cast<JobDetail>().ToList();
    }

    public JobDetail? GetJob(string jobId)
    {
        return _jobDetailRepo.CopyNew().GetFirst(o => o.JobId == jobId);
    }

    public void AddOrUpdateJob(JobDetail job)
    {
        var entity = job.Adapt<SysJobDetail>();
        entity.Id = _jobDetailRepo.CopyNew().GetFirst(o => o.JobId == job.JobId)?.Id ?? 0;
        _jobDetailRepo.CopyNew().InsertOrUpdate(entity);
    }

    public void RemoveJob(string jobId)
    {
        _jobDetailRepo.CopyNew().AsDeleteable()
            .Where(o => o.JobId == jobId)
            .ExecuteCommand();
    }

    public List<JobTrigger> GetTriggers()
    {
        var list = _triggerRepo.CopyNew().GetList();
        return list.Cast<JobTrigger>().ToList();
    }

    public List<JobTrigger> GetTriggers(string jobId)
    {
        var list = _triggerRepo.CopyNew().GetList(o => o.JobId == jobId);
        return list.Cast<JobTrigger>().ToList();
    }

    public JobTrigger? GetTrigger(string jobId, string triggerId)
    {
        return _triggerRepo.CopyNew().GetFirst(o => o.JobId == jobId && o.TriggerId == triggerId);
    }

    public void AddOrUpdateTrigger(JobTrigger trigger)
    {
        var entity = trigger.Adapt<SysJobTrigger>();
        entity.Id = _triggerRepo.CopyNew().GetFirst(o => o.JobId == trigger.JobId && o.TriggerId == trigger.TriggerId)?.Id ?? 0;
        _triggerRepo.CopyNew().InsertOrUpdate(entity);
    }

    public void RemoveTrigger(string jobId, string triggerId)
    {
        _triggerRepo.CopyNew().AsDeleteable()
            .Where(o => o.JobId == jobId && o.TriggerId == triggerId)
            .ExecuteCommand();
    }
}
