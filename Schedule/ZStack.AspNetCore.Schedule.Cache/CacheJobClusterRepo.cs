using NewLife.Caching;

namespace ZStack.AspNetCore.Schedule.Cache;

public class CacheJobClusterRepo(ICache _cache) : IJobClusterRepo
{
    private readonly string _prefix = "Job:Cluster:";

    public List<JobCluster> List()
    {
        var keys = _cache.Keys(_prefix).ToArray();
        if (keys.Length == 0) return [];
        return _cache.GetAll<JobCluster>(keys)
            .Values
            .Where(x => x != null)
            .ToList()!;
    }

    public Task<List<JobCluster>> ListAsync()
    {
        return Task.FromResult(List());
    }

    public void AddOrUpdate(JobCluster cluster)
    {
        _cache.Add(_prefix + cluster.ClusterId, cluster);
    }

    public Task AddOrUpdateAsync(JobCluster cluster)
    {
        AddOrUpdate(cluster);
        return Task.CompletedTask;
    }

    public JobCluster? Get(string clusterId)
    {
        var key = _prefix + clusterId;
        if (_cache.ContainsKey(key))
            return _cache.Get<JobCluster>(key);
        return null;
    }

    public Task<JobCluster?> GetAsync(string clusterId)
    {
        return Task.FromResult(Get(clusterId));
    }

    public void Remove(string clusterId)
    {
        _cache.Remove(_prefix + clusterId);
    }

    public Task RemoveAsync(string clusterId)
    {
        Remove(clusterId);
        return Task.CompletedTask;
    }
}
