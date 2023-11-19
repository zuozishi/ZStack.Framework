namespace ZStack.AspNetCore.SqlSugar;

public interface ISqlSugarService
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    DbConnectionOptions Options { get; }

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    SqlSugarScope Get(string? configId = null);
}
