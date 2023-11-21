using Mapster;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using ZStack.AspNetCore.SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

public class SqlSugarJobClusterRepo : IJobClusterRepo
{
    private readonly SimpleClient<SysJobCluster> _repo;

    public SqlSugarJobClusterRepo(IConfiguration configuration, ISqlSugarService sqlSugarService)
    {
        var configId = configuration.Get<string?>("Schedule:SqlSugarConfigId");
        var db = sqlSugarService.Get(configId);
        db.CodeFirst.SetStringDefaultLength(128)
            .InitTables<SysJobCluster>();
        _repo = db.GetSimpleClient<SysJobCluster>();
    }

    public List<JobCluster> List()
    {
        var list = _repo.CopyNew().GetList();
        return list.Cast<JobCluster>().ToList();
    }

    public async Task<List<JobCluster>> ListAsync()
    {
        var list = await _repo.GetListAsync();
        return list.Cast<JobCluster>().ToList();
    }

    public JobCluster? Get(string clusterId)
    {
        return _repo.CopyNew().GetFirst(o => o.ClusterId == clusterId);
    }

    public async Task<JobCluster?> GetAsync(string clusterId)
    {
        return await _repo.GetFirstAsync(o => o.ClusterId == clusterId);
    }

    public void AddOrUpdate(JobCluster cluster)
    {
        var entity = cluster.Adapt<SysJobCluster>();
        entity.Id = _repo.CopyNew().GetFirst(o => o.ClusterId == cluster.ClusterId)?.Id ?? 0;
        _repo.CopyNew().InsertOrUpdate(entity);
    }

    public async Task AddOrUpdateAsync(JobCluster cluster)
    {
        var entity = cluster.Adapt<SysJobCluster>();
        entity.Id = (await _repo.GetFirstAsync(o => o.ClusterId == cluster.ClusterId))?.Id ?? 0;
        await _repo.InsertOrUpdateAsync(entity);
    }

    public void Remove(string clusterId)
    {
        _repo.CopyNew().AsDeleteable()
            .Where(o => o.ClusterId == clusterId)
            .ExecuteCommand();
    }

    public async Task RemoveAsync(string clusterId)
    {
        await _repo.AsDeleteable()
            .Where(o => o.ClusterId == clusterId)
            .ExecuteCommandAsync();
    }
}
