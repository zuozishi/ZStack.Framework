using System.Diagnostics.CodeAnalysis;

namespace ZStack.AspNetCore.SqlSugar;

public interface ISqlSugarService
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    DbConnectionOptions Options { get; }

    /// <summary>
    /// 是否包含指定的数据库配置
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    bool Contains(string configId = SqlSugarConst.MainConfigId);

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    SqlSugarScope Get(string? configId = null);

    /// <summary>
    /// 尝试获取数据库上下文
    /// </summary>
    /// <param name="configId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    bool TryGet(string? configId, [MaybeNullWhen(false)] out SqlSugarScope scope);
}
