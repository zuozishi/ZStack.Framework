using Mapster;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NewLife.Caching;
using ZStack.AspNetCore.Options;
using ZStack.Core.Exceptions;

namespace ZStack.AspNetCore.SqlSugar;

/// <summary>
/// 分布式WorkId生成器
/// </summary>
/// <param name="_logger"></param>
/// <param name="_cacheOptions"></param>
/// <param name="_snowIdOptions"></param>
public class IdGeneratorWorker(ILogger<IdGeneratorWorker> _logger, IOptions<CacheOptions> _cacheOptions, IOptions<SnowIdOptions> _snowIdOptions) : BackgroundService
{
    private FullRedis? redisClient;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_cacheOptions.Value.CacheType != CacheTypes.Redis || _cacheOptions.Value.Redis == null)
            return Task.CompletedTask;
        var redisOption = _cacheOptions.Value.Redis.Adapt<RedisOptions>();
        redisOption.Db = 0;
        redisOption.Prefix = null;
        redisClient = new FullRedis(redisOption);
        var workerId = GetLook()
            ?? throw Oops.Bah("IdGenerator配置失败, 无可用WorkerId");
        _logger.LogInformation("IdGenerator WorkerId: {WorkerId}", workerId);
        _snowIdOptions.Value.WorkerId = workerId;
        YitIdHelper.SetIdGenerator(_snowIdOptions.Value);
        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
                GetLook(workerId);
            }
        }, cancellationToken: stoppingToken);
    }

    private ushort? GetLook(ushort? workerId = null)
    {
        if (redisClient == null)
            return null;
        if (workerId != null)
        {
            redisClient.Set($"SnowId:Lock:{workerId}", Environment.MachineName, 15);
            return workerId;
        }
        for (int i = 1; i <= 9; i++)
        {
            var key = $"SnowId:Lock:{i}";
            if (!redisClient.ContainsKey(key))
            {
                redisClient.Set(key, Environment.MachineName, 15);
                return (ushort)i;
            }
        }
        return null;
    }

    public override void Dispose()
    {
        redisClient?.Dispose();
        GC.SuppressFinalize(this);
        base.Dispose();
    }
}
