using Microsoft.Extensions.Options;

namespace ZStack.AspNetCore.SqlSugar.Options;

/// <summary>
/// 数据库配置选项
/// </summary>
public class DbConnectionOptions : IConfigureOptions<DbConnectionOptions>
{
    /// <summary>
    /// 执行超时时间(秒)
    /// </summary>
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// 数据库集合
    /// </summary>
    public List<DbConnectionConfig> ConnectionConfigs { get; set; } = [];

    public void Configure(DbConnectionOptions options)
    {
        foreach (var dbConfig in options.ConnectionConfigs)
        {
            if (string.IsNullOrWhiteSpace(dbConfig.ConfigId?.ToString()))
                dbConfig.ConfigId = SqlSugarConst.MainConfigId;
        }
    }
}

/// <summary>
/// 数据库连接配置
/// </summary>
public sealed class DbConnectionConfig : ConnectionConfig
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    public DbSettings DbSettings { get; set; } = new();

    /// <summary>
    /// 表设置
    /// </summary>
    public TableSettings TableSettings { get; set; } = new();

    /// <summary>
    /// 种子设置
    /// </summary>
    public SeedSettings SeedSettings { get; set; } = new();

    /// <summary>
    /// 拦截器设置
    /// </summary>
    public AopSettings AopSettings { get; set; } = new();
}

/// <summary>
/// 数据库设置
/// </summary>
public sealed class DbSettings
{
    /// <summary>
    /// 启用库表初始化
    /// </summary>
    public bool EnableInitDb { get; set; }

    /// <summary>
    /// 启用驼峰转下划线
    /// </summary>
    public bool EnableUnderLine { get; set; }
}

/// <summary>
/// 表设置
/// </summary>
public sealed class TableSettings
{
    /// <summary>
    /// 启用表初始化
    /// </summary>
    public bool EnableInitTable { get; set; }

    /// <summary>
    /// 启用表增量更新
    /// </summary>
    public bool EnableIncreTable { get; set; }
}

/// <summary>
/// 种子设置
/// </summary>
public sealed class SeedSettings
{
    /// <summary>
    /// 启用种子初始化
    /// </summary>
    public bool EnableInitSeed { get; set; }

    /// <summary>
    /// 启用种子增量更新
    /// </summary>
    public bool EnableIncreSeed { get; set; }
}

/// <summary>
/// 拦截器设置
/// </summary>
public sealed class AopSettings
{
    /// <summary>
    /// 启用Sql日志，默认：否
    /// </summary>
    public bool EnableSqlLog { get; set; } = false;

    /// <summary>
    /// 启用Sql错误日志，默认：是
    /// </summary>
    public bool EnableErrorSqlLog { get; set; } = true;

    /// <summary>
    /// 启用慢Sql日志，默认：否
    /// </summary>
    public bool EnableSlowSqlLog { get; set; } = false;

    /// <summary>
    ///  慢Sql时间(毫秒)，默认：5000
    /// </summary>
    public long SlowSqlTime { get; set; } = 5000;

    /// <summary>
    /// 启用Id自动填充，默认：是
    /// </summary>
    public bool EnableIdAutoFill { get; set; } = true;

    /// <summary>
    /// 创建时间字段
    /// </summary>
    public List<string> CreateTimeFields { get; set; } = [];

    /// <summary>
    /// 更新时间字段
    /// </summary>
    public List<string> UpdateTimeFields { get; set; } = [];
}
