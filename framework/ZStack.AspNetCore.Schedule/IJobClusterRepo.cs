namespace ZStack.AspNetCore.Schedule;

public interface IJobClusterRepo
{
    List<JobCluster> List();

    Task<List<JobCluster>> ListAsync();

    JobCluster? Get(string clusterId);

    Task<JobCluster?> GetAsync(string clusterId);

    void AddOrUpdate(JobCluster cluster);

    Task AddOrUpdateAsync(JobCluster cluster);

    void Remove(string clusterId);

    Task RemoveAsync(string clusterId);
}
