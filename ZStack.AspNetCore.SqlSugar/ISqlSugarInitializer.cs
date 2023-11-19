namespace ZStack.AspNetCore.SqlSugar;

public interface ISqlSugarInitializer
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    DbConnectionOptions Options { get; }

    /// <summary>
    /// 初始化数据库连接配置
    /// </summary>
    /// <param name="config"></param>
    void SetDbConfig(DbConnectionConfig config);

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="config"></param>
    /// <param name="db"></param>
    void InitDatabase(DbConnectionConfig config, SqlSugarScope db);

    /// <summary>
    /// 配置过滤器
    /// </summary>
    /// <param name="db"></param>
    void SetDbAop(DbConnectionConfig config, SqlSugarClient db);
}
