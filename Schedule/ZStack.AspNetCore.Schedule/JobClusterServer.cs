using Furion.Schedule;
using NewLife.Caching;

namespace ZStack.AspNetCore.Schedule;

public class JobClusterServer(IJobClusterRepo _repo, ICache _cache) : IJobClusterServer
{
    private readonly Random rd = new(DateTime.Now.Millisecond);

    public void Start(JobClusterContext context)
    {
        var cluster = _repo.Get(context.ClusterId);
        if (cluster == null)
        {
            cluster = new JobCluster
            {
                ClusterId = context.ClusterId,
                Status = ClusterStatus.Waiting,
                UpdatedTime = DateTime.Now
            };
            _repo.AddOrUpdate(cluster);
        }
        else
        {
            cluster.Status = ClusterStatus.Waiting;
            cluster.UpdatedTime = DateTime.Now;
            _repo.AddOrUpdate(cluster);
        }
    }

    public async Task WaitingForAsync(JobClusterContext context)
    {
        var clusterId = context.ClusterId;
        while (true)
        {
            // 控制集群心跳频率（放在头部为了防止 IsAnyAsync continue 没sleep占用大量IO和CPU）
            await Task.Delay(3000 + rd.Next(500, 1000)); // 错开集群同时启动
            try
            {
                using (_cache.AcquireLock("lock:JobClusterServer:WaitingForAsync", 1000))
                {
                    var clusters = await _repo.ListAsync();
                    if (clusters.Any(x => x.Status == ClusterStatus.Working))
                        continue;
                    var cluster = clusters.FirstOrDefault(x => x.ClusterId == clusterId);
                    if (cluster != null)
                    {
                        cluster.Status = ClusterStatus.Working;
                        cluster.UpdatedTime = DateTime.Now;
                        await _repo.AddOrUpdateAsync(cluster);
                    }
                    return;
                }
            }
            catch { }
        }
    }

    public void Stop(JobClusterContext context)
    {
        var cluster = _repo.Get(context.ClusterId);
        if (cluster != null)
        {
            cluster.Status = ClusterStatus.Waiting;
            cluster.UpdatedTime = DateTime.Now;
            _repo.AddOrUpdate(cluster);
        }
    }

    public void Crash(JobClusterContext context)
    {
        var cluster = _repo.Get(context.ClusterId);
        if (cluster != null)
        {
            cluster.Status = ClusterStatus.Crashed;
            cluster.UpdatedTime = DateTime.Now;
            _repo.AddOrUpdate(cluster);
        }
    }
}
