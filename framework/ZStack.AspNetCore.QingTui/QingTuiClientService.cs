using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using ZStack.Core.Exceptions;
using ZStack.QingTui;

namespace ZStack.AspNetCore.QingTui;

/// <summary>
/// 轻推API客户端服务
/// </summary>
internal class QingTuiClientService : IQingTuiClientService
{
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, QingTuiApiClient> _pool = [];

    public QingTuiClientService(ILogger<QingTuiClientService> logger, ITokenPersister tokenPersister, IOptions<QingTuiClientOptions> options)
    {
        _logger = logger;
        foreach (var clientOptions in options.Value.Clients)
        {
            var builder = new QingTuiApiClientOptionsBuilder()
                .SetDisplayName(clientOptions.DisplayName)
                .SetAppId(clientOptions.AppId)
                .SetSecret(clientOptions.Secret)
                .SetLogger(logger)
                .SetTokenPersister(tokenPersister);
            if (!string.IsNullOrEmpty(options.Value.Host))
                builder.SetHost(options.Value.Host);
            if (_pool.ContainsKey(clientOptions.AppId))
                throw Oops.Bah($"重复的appId: {clientOptions.AppId}");
            logger.LogInformation("注册轻推API客户端: [{Appid}] {DisplayName}", clientOptions.AppId, clientOptions.DisplayName);
            _pool.TryAdd(clientOptions.AppId, new QingTuiApiClient(builder.Build()));
        }
    }

    public QingTuiApiClient Get(string appId)
    {
        if (_pool.TryGetValue(appId, out QingTuiApiClient? client))
            return client;
        throw Oops.Bah($"未找到对应的轻推API客户端实例, appId={appId}");
    }

    public void Add(QingTuiApiClient client)
    {
        if (_pool.ContainsKey(client.AppId))
            throw Oops.Bah($"重复的appId: {client.AppId}");
        _logger.LogInformation("注册轻推API客户端: [{Appid}] {DisplayName}", client.AppId, client.DisplayName);
        _pool.TryAdd(client.AppId, client);
    }
}
