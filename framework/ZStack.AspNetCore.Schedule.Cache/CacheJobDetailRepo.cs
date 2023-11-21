using NewLife.Caching;

namespace ZStack.AspNetCore.Schedule.Cache;

public class CacheJobDetailRepo(ICache _cache) : IJobDetailRepo
{
    private readonly string _jobDetailPrefix = "Job:Detail:";
    private readonly string _jobTriggerPrefix = "Job:Trigger:";

    public List<JobDetail> GetJobs()
    {
        var keys = _cache.Search(_jobDetailPrefix + "*");
        var jobs = new List<JobDetail>();
        foreach (var key in keys)
        {
            var job = _cache.Get<JobDetail>(key);
            if (job != null)
                jobs.Add(job);
        }
        return jobs;
    }

    public JobDetail? GetJob(string jobId)
    {
        return _cache.Get<JobDetail>(_jobDetailPrefix + jobId);
    }

    public void AddOrUpdateJob(JobDetail job)
    {
        _cache.Set(_jobDetailPrefix + job.JobId, job);
    }

    public void RemoveJob(string jobId)
    {
        _cache.Remove(_jobDetailPrefix + jobId);
    }

    public List<JobTrigger> GetTriggers()
    {
        var keys = _cache.Search(_jobTriggerPrefix + "*");
        var triggers = new List<JobTrigger>();
        foreach (var key in keys)
        {
            var trigger = _cache.Get<JobTrigger>(key);
            if (trigger != null)
                triggers.Add(trigger);
        }
        return triggers;
    }

    public List<JobTrigger> GetTriggers(string jobId)
        => GetTriggers().Where(x => x.JobId == jobId).ToList();

    public JobTrigger? GetTrigger(string jobId, string triggerId)
    {
        return _cache.Get<JobTrigger>(_jobTriggerPrefix + jobId + ":" + triggerId);
    }

    public void AddOrUpdateTrigger(JobTrigger trigger)
    {
        _cache.Set(_jobTriggerPrefix + trigger.JobId + ":" + trigger.TriggerId, trigger);
    }

    public void RemoveTrigger(string jobId, string triggerId)
    {
        _cache.Remove(_jobTriggerPrefix + jobId + ":" + triggerId);
    }
}
