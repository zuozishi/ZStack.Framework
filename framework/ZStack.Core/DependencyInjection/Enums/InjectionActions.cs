using System.ComponentModel;

namespace ZStack.Core.DependencyInjection;

/// <summary>
/// 服务注册方式
/// </summary>
public enum InjectionActions
{
    /// <summary>
    /// 如果存在则覆盖
    /// </summary>
    [Description("存在则覆盖")]
    Add,

    /// <summary>
    /// 如果存在则跳过，默认方式
    /// </summary>
    [Description("存在则跳过")]
    TryAdd
}
