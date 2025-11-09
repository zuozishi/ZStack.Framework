using System.Data.Common;
using System.Diagnostics;

namespace ZStack.SqlSugar;

internal static class SqlSugarActivitySource
{
    static readonly ActivitySource Source = new(SqlSugarConst.ActiveName, "0.1.0");

    private readonly static ConcurrentDictionary<Guid, Activity> _pools = [];
    private readonly static ConcurrentDictionary<string, DbConnectionStringBuilder> _connStringCache = [];

    internal static void CommandStart(SqlSugarClient db, string sqlText)
    {
        if (!Source.HasListeners())
            return;

        var config = db.CurrentConnectionConfig;
        var configId = config.ConfigId?.ToString() ?? "default";
        var activity = Source.StartActivity("SqlSugar-" + configId, ActivityKind.Client);
        if (activity is not { IsAllDataRequested: true })
            return;

        var connStringBuilder = _connStringCache.GetOrAdd(db.CurrentConnectionConfig.ConnectionString, (_) =>
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = db.CurrentConnectionConfig.ConnectionString };
            builder.Remove("password");
            builder.Remove("pwd");
            return builder;
        });

        activity.SetTag("db.connection_id", db.ContextID);
        activity.SetTag("db.connection_string", connStringBuilder.ConnectionString);
        if (connStringBuilder.TryGetValue("database", out var database))
            activity.SetTag("db.name", database);
        activity.SetTag("db.statement", sqlText);
        activity.SetTag("db.system", config.DbType.ToString().ToLower());
        if (connStringBuilder.TryGetValue("uid", out var uid))
            activity.SetTag("db.user", uid);
        if (connStringBuilder.TryGetValue("user id", out var userId))
            activity.SetTag("db.user", userId);
        if (connStringBuilder.TryGetValue("server", out var server))
        {
            activity.SetTag("net.peer.ip", server);
            activity.SetTag("net.peer.name", server);
            activity.SetTag("net.transport", "ip_tcp");
        }
        if (connStringBuilder.TryGetValue("host", out var host))
        {
            activity.SetTag("net.peer.ip", host);
            activity.SetTag("net.peer.name", host);
            activity.SetTag("net.transport", "ip_tcp");
        }
        if (connStringBuilder.TryGetValue("port", out var port))
            activity.SetTag("net.peer.port", port);
        _pools.AddOrUpdate(db.ContextID, activity, (_, oldValue) =>
        {
            oldValue.Dispose();
            return activity;
        });
    }

    internal static void CommandStop(SqlSugarClient db)
    {
        if (!Source.HasListeners())
            return;
        if (_pools.ContainsKey(db.ContextID))
        {
            _pools.TryRemove(db.ContextID, out var activity);
            activity?.SetTag("otel.status_code", "OK");
            activity?.Dispose();
        }
    }

    internal static void SetException(SqlSugarClient db, SqlSugarException ex)
    {
        if (!Source.HasListeners())
            return;
        if (_pools.ContainsKey(db.ContextID))
        {
            _pools.TryRemove(db.ContextID, out var activity);
            var tags = new ActivityTagsCollection
            {
                { "exception.type", ex.GetType().FullName },
                { "exception.message", ex.Message },
                { "exception.stacktrace", ex.ToString() }
            };
            var activityEvent = new ActivityEvent("exception", tags: tags);
            activity?.AddEvent(activityEvent);
            activity?.SetTag("otel.status_code", "ERROR");
            activity?.SetTag("otel.status_description", ex.Message);
            activity?.Dispose();
        }
    }
}
