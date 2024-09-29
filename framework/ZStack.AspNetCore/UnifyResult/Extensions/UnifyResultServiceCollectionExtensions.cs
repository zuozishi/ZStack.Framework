using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using ZStack.AspNetCore.UnifyResult;

namespace Microsoft.Extensions.DependencyInjection;

public static class UnifyResultServiceCollectionExtensions
{
    /// <summary>
    /// 添加规范化结果服务
    /// </summary>
    /// <param name="mvcBuilder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddUnifyResult(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.Services.AddUnifyResult<RESTfulResultProvider>();

        return mvcBuilder;
    }

    /// <summary>
    /// 添加规范化结果服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnifyResult(this IServiceCollection services)
    {
        services.AddUnifyResult<RESTfulResultProvider>();

        return services;
    }

    /// <summary>
    /// 添加规范化结果服务
    /// </summary>
    /// <typeparam name="TUnifyResultProvider"></typeparam>
    /// <param name="mvcBuilder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddUnifyResult<TUnifyResultProvider>(this IMvcBuilder mvcBuilder)
        where TUnifyResultProvider : class, IUnifyResultProvider
    {
        mvcBuilder.Services.AddUnifyResult<TUnifyResultProvider>();

        return mvcBuilder;
    }

    /// <summary>
    /// 添加规范化结果服务
    /// </summary>
    /// <typeparam name="TUnifyResultProvider"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnifyResult<TUnifyResultProvider>(this IServiceCollection services)
        where TUnifyResultProvider : class, IUnifyResultProvider
    {
        // 是否启用规范化结果
        UnifyContext.EnabledUnifyHandler = true;

        // 添加规范化提供器
        services.AddUnifyProvider<TUnifyResultProvider>(string.Empty);

        // 添加成功规范化结果筛选器
        services.Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new UnifyResultModelConvention());
            options.Filters.Add<UnifyResultFilter>();
        });

        // 添加模型验证失败规范化结果筛选器
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                if (!UnifyContext.CheckStatusCodeNonUnify(context.HttpContext, out var unifyResult))
                {
                    if (unifyResult != null)
                        return unifyResult.OnValidateFailed(context);
                }
                return new BadRequestObjectResult(context.ModelState);
            };
        });

        // 添加状态码拦截中间件
        services.AddScoped<UnifyResultStatusCodesMiddleware>();

        return services;
    }

    /// <summary>
    /// 替换默认的规范化结果
    /// </summary>
    /// <typeparam name="TUnifyResultProvider"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnifyProvider<TUnifyResultProvider>(this IServiceCollection services)
        where TUnifyResultProvider : class, IUnifyResultProvider
    {
        return services.AddUnifyProvider<TUnifyResultProvider>(string.Empty);
    }

    /// <summary>
    /// 添加规范化提供器
    /// </summary>
    /// <typeparam name="TUnifyResultProvider"></typeparam>
    /// <param name="services"></param>
    /// <param name="providerName"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnifyProvider<TUnifyResultProvider>(this IServiceCollection services, string providerName)
        where TUnifyResultProvider : class, IUnifyResultProvider
    {
        var providerType = typeof(TUnifyResultProvider);

        // 添加规范化提供器
        services.TryAddSingleton(providerType);

        // 获取规范化提供器模型，不能为空
        var resultType = providerType.GetCustomAttribute<UnifyModelAttribute>()?.ModelType
            ?? throw new ArgumentException("添加规范化提供器必须设置UnifyModelAttribute");

        // 创建规范化元数据
        var metadata = new UnifyMetadata(providerName, providerType, resultType);

        // 添加或替换规范化配置
        UnifyContext.UnifyProviders.AddOrUpdate(providerName, _ => metadata, (_, _) => metadata);

        return services;
    }
}
