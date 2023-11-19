namespace ZStack.AspNetCore.Schedule;

public interface IJobDetailRepo
{
    public List<JobDetail> GetJobs();

    public JobDetail? GetJob(string jobId);

    public void AddOrUpdateJob(JobDetail job);

    public void RemoveJob(string jobId);

    public List<JobTrigger> GetTriggers();

    public List<JobTrigger> GetTriggers(string jobId);

    public JobTrigger? GetTrigger(string jobId, string triggerId);

    public void AddOrUpdateTrigger(JobTrigger trigger);

    public void RemoveTrigger(string jobId, string triggerId);
}
