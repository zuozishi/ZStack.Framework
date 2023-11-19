namespace ZStack.AspNetCore.SqlSugar;

public class SqlSugarService : ISqlSugarService
{
    public DbConnectionOptions Options { get; } = App.GetOptions<DbConnectionOptions>();

    private readonly ILogger _logger;
    private readonly ISqlSugarInitializer _initializer;
    private readonly ConcurrentDictionary<string, SqlSugarScope> _scopes = [];

    public SqlSugarService(ILogger<SqlSugarService> logger, ISqlSugarInitializer initializer)
    {
        _logger = logger;
        _initializer = initializer;
        if (Options.ConnectionConfigs.GroupBy(x => x.ConfigId).Any(x => x.Count() > 1))
            throw Oops.Bah("存在重复的数据库配置项, 请检查 ConfigId 是否重复");
    }

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
            ?? throw Oops.Bah("数据源配置不存在, configId={ConfigId}", configId);
        _initializer.SetDbConfig(config);
        scope = new SqlSugarScope(config, db =>
        {
            _initializer.SetDbAop(config, db);
        });
        _initializer.InitDatabase(config, scope);
        _scopes.AddOrUpdate(configId, scope, (_, _) => scope);
        return scope;
    }
}
