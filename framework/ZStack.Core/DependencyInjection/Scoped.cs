using Microsoft.Extensions.DependencyInjection;

namespace ZStack.Core.DependencyInjection;

/// <summary>
/// 创建作用域静态类
/// </summary>
public static partial class Scoped
{
    /// <summary>
    /// 创建一个作用域范围
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="scopeFactory"></param>
    public static void Create(Action<IServiceScopeFactory, IServiceScope> handler, IServiceScopeFactory scopeFactory)
    {
        CreateAsync(async (fac, scope) =>
        {
            handler(fac, scope);
            await Task.CompletedTask;
        }, scopeFactory).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 创建一个作用域范围（异步）
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="scopeFactory"></param>
    public static async Task CreateAsync(Func<IServiceScopeFactory, IServiceScope, Task> handler, IServiceScopeFactory scopeFactory)
    {
        // 禁止空调用
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(scopeFactory);

        // 创建作用域
        var scope = scopeFactory.CreateScope();

        try
        {
            // 执行方法
            await handler(scopeFactory, scope);
        }
        catch
        {
            throw;
        }
        finally
        {
            // 释放
            scope.Dispose();
        }
    }
}
