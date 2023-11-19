﻿using Furion.Schedule;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ZStack.AspNetCore.Schedule.Dashboard;

/// <summary>
/// 构造函数
/// </summary>
/// <param name="next">请求委托</param>
/// <param name="schedulerFactory">作业计划工厂</param>
/// <param name="options">UI 配置选项</param>
public sealed class ScheduleUIMiddleware(RequestDelegate next
        , ISchedulerFactory schedulerFactory
        , ScheduleUIOptions options)
{
    /// <summary>
    /// 请求委托
    /// </summary>
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// 作业计划工厂
    /// </summary>
    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

    /// <summary>
    /// UI 配置选项
    /// </summary>
    public ScheduleUIOptions Options { get; } = options;

    /// <summary>
    /// API 入口地址
    /// </summary>
    public string ApiRequestPath { get; } = $"{options.RequestPath}/api";

    /// <summary>
    /// 中间件执行方法
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/></param>
    /// <returns><see cref="Task"/></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // 非看板请求跳过
        if (!context.Request.Path.StartsWithSegments(Options.RequestPath, StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var asd = context.Request.Path;

        // ================================ 处理 API 请求 ================================

        // 如果不是以 API_REQUEST_PATH 开头，则跳过
        if (!context.Request.Path.StartsWithSegments(ApiRequestPath, StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // 只处理 GET/POST 请求
        if (!context.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase) && !context.Request.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
        {
            await _next(context);
            return;
        }

        // 获取匹配的路由标识
        var action = context.Request.Path.Value?[ApiRequestPath.Length..]?.ToLower();

        // 允许跨域，设置返回 json
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.Headers.AccessControlAllowHeaders = "*";

        // 路由匹配
        switch (action)
        {
            // 获取所有作业
            case "/get-jobs":
                var jobs = _schedulerFactory.GetJobsOfModels().OrderBy(u => u.JobDetail.GroupName);

                // 输出 JSON
                await context.Response.WriteAsync(SerializeToJson(jobs));
                break;
            // 操作作业
            case "/operate-job":
                // 获取作业 Id
                var jobId = context.Request.Query["jobid"];
                // 获取操作方法
                var operate = context.Request.Query["action"];

                // 获取作业计划
                var scheduleResult = _schedulerFactory.TryGetJob(jobId, out var scheduler);

                // 处理找不到作业情况
                if (scheduleResult != ScheduleResult.Succeed)
                {
                    // 标识状态码为 500
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // 输出 JSON
                    await context.Response.WriteAsync(SerializeToJson(new
                    {
                        msg = scheduleResult.ToString(),
                        ok = false
                    }));

                    return;
                }

                switch (operate)
                {
                    // 启动作业
                    case "start":
                        scheduler?.Start();
                        break;
                    // 暂停作业
                    case "pause":
                        scheduler?.Pause();
                        break;
                    // 移除作业
                    case "remove":
                        _schedulerFactory.RemoveJob(jobId);
                        break;
                    // 立即执行
                    case "run":
                        _schedulerFactory.RunJob(jobId);
                        break;
                }

                // 输出 JSON
                await context.Response.WriteAsync(SerializeToJson(new
                {
                    msg = ScheduleResult.Succeed.ToString(),
                    ok = true
                }));

                break;
            // 操作触发器
            case "/operate-trigger":
                // 获取作业 Id
                var jobId1 = context.Request.Query["jobid"];
                var triggerId = context.Request.Query["triggerid"];
                // 获取操作方法
                var operate1 = context.Request.Query["action"];

                // 获取作业计划
                var scheduleResult1 = _schedulerFactory.TryGetJob(jobId1, out var scheduler1);

                // 处理找不到作业情况
                if (scheduleResult1 != ScheduleResult.Succeed)
                {
                    // 标识状态码为 500
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // 输出 JSON
                    await context.Response.WriteAsync(SerializeToJson(new
                    {
                        msg = scheduleResult1.ToString(),
                        ok = false
                    }));

                    return;
                }

                switch (operate1)
                {
                    // 启动作业触发器
                    case "start":
                        scheduler1?.StartTrigger(triggerId);
                        break;
                    // 暂停作业触发器
                    case "pause":
                        scheduler1?.PauseTrigger(triggerId);
                        break;
                    // 移除作业触发器
                    case "remove":
                        scheduler1?.RemoveTrigger(triggerId);
                        break;
                    // 获取作业触发器最近运行时间
                    case "timelines":
                        var trigger = scheduler1?.GetTrigger(triggerId);
                        var timelines = trigger?.GetTimelines() ?? Array.Empty<TriggerTimeline>();

                        // 输出 JSON
                        await context.Response.WriteAsync(SerializeToJson(timelines));
                        return;
                }

                // 输出 JSON
                await context.Response.WriteAsync(SerializeToJson(new
                {
                    msg = ScheduleResult.Succeed.ToString(),
                    ok = true
                }));

                break;

            // 推送更新
            case "/check-change":
                // 检查请求类型，是否为 text/event-stream 格式
                if (!context.WebSockets.IsWebSocketRequest && context.Request.Headers["Accept"].ToString().Contains("text/event-stream"))
                {
                    // 设置响应头的 content-type 为 text/event-stream
                    context.Response.ContentType = "text/event-stream";

                    // 设置响应头，启用响应发送保持活动性
                    context.Response.Headers.Append("Cache-Control", "no-cache");
                    context.Response.Headers.Append("Connection", "keep-alive");

                    var queue = new BlockingCollection<Furion.Schedule.JobDetail>();

                    // 监听作业计划变化
                    void Subscribe(object sender, SchedulerEventArgs args)
                    {
                        if (!queue.IsAddingCompleted)
                        {
                            queue.Add(args.JobDetail);
                        }
                    }
                    _schedulerFactory.OnChanged += Subscribe!;

                    // 持续发送 SSE 协议数据
                    foreach (var jobDetail in queue.GetConsumingEnumerable())
                    {
                        // 如果请求已终止则停止推送
                        if (!context.RequestAborted.IsCancellationRequested)
                        {
                            var message = "data: " + SerializeToJson(jobDetail) + "\n\n";
                            await context.Response.WriteAsync(message);
                            //await context.Response.Body.FlushAsync();
                        }
                        else break;
                    }

                    queue.CompleteAdding();
                    _schedulerFactory.OnChanged -= Subscribe!;
                }
                break;
        }
    }

    private readonly static JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// 将对象输出为 JSON 字符串
    /// </summary>
    /// <param name="obj">对象</param>
    /// <returns><see cref="string"/></returns>
    private static string SerializeToJson(object obj)
    {
        return JsonSerializer.Serialize(obj, _serializerOptions);
    }
}
