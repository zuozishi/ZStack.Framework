using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using ZStack.Core.Exceptions;
using ZStack.SqlSugar.Options;

namespace ZStack.SqlSugar;

public class SqlSugarService : ISqlSugarService
{
    public DbConnectionOptions Options { get; }

    private readonly ILogger _logger;
    private readonly ISqlSugarInitializer _initializer;
    private readonly ConcurrentDictionary<string, SqlSugarScope> _scopes = [];

    public SqlSugarService(ILogger<SqlSugarService> logger, ISqlSugarInitializer initializer, IOptions<DbConnectionOptions> options)
    {
        _logger = logger;
        _initializer = initializer;
        Options = options.Value;
        if (Options.ConnectionConfigs.GroupBy(x => x.ConfigId).Any(x => x.Count() > 1))
            throw Oops.Bah("存在重复的数据库配置项, 请检查 ConfigId 是否重复");
    }

    /// <summary>
    /// 是否包含指定的数据库配置
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public bool Contains(string configId = SqlSugarConst.MainConfigId)
        => Options.ConnectionConfigs.Any(x => x.ConfigId?.ToString() == configId);

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public SqlSugarScope Get(string? configId = null)
    {
        configId ??= SqlSugarConst.MainConfigId;
        if (_scopes.TryGetValue(configId, out var scope))
            return scope;
        var config = Options.ConnectionConfigs.FirstOrDefault(x => x.ConfigId?.ToString() == configId)
            ?? throw Oops.Bah($"数据源配置不存在, configId={configId}");
        _initializer.SetDbConfig(config);
        scope = new SqlSugarScope(config, db =>
        {
            _initializer.SetDbAop(config, db);
            Options.OnSqlSugarClientConfigure?.Invoke(config, db);
        });
        _initializer.InitDatabase(config, scope);
        _logger.LogInformation("注册数据库上下文, configId={configId}, dbType={DbType}", configId, config.DbType);
        _scopes.AddOrUpdate(configId, scope, (_, _) => scope);
        return scope;
    }

    /// <summary>
    /// 尝试获取数据库上下文
    /// </summary>
    /// <param name="configId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    public bool TryGet(string? configId, [MaybeNullWhen(false)] out SqlSugarScope scope)
    {
        if (configId == null)
        {
            scope = Get();
            return true;
        }
        return _scopes.TryGetValue(configId, out scope);
    }
}
