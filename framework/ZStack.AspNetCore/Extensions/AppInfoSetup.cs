using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Microsoft.AspNetCore.Builder;

public static class AppInfoSetup
{
    /// <summary>
    /// 显示应用程序信息，包括监听的地址和端口
    /// </summary>
    public static IApplicationBuilder PrintAppEndpoints(this IApplicationBuilder app)
    {
        var addresses = app.ApplicationServices
            .GetRequiredService<IServer>().Features
            .Get<IServerAddressesFeature>()?.Addresses;

        app.ApplicationServices
            .GetRequiredService<IHostApplicationLifetime>()
            .ApplicationStarted.Register(() =>
            {
                var logger = app.ApplicationServices.GetRequiredService<ILogger<App>>();
                var endpoints = GetListeningAddresses(addresses);
                if (endpoints.Length > 0)
                    logger.LogInformation("Application listening on:\n{Urls}",
                        string.Join("\n", endpoints.Select(url => $"\t{url}")));
            });

        return app;
    }

    static string[] GetListeningAddresses(IEnumerable<string>? serverAddresses)
    {
        if (serverAddresses is null)
            return [];

        var localIPs = GetLocalIPAddresses();
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var address in serverAddresses)
        {
            if (!Uri.TryCreate(address, UriKind.Absolute, out var uri))
                continue;

            var (scheme, host, port) = (uri.Scheme, uri.Host, uri.Port);

            if (host is "*" or "+" or "0.0.0.0" or "[::]")
            {
                result.Add($"{scheme}://localhost:{port}");
                foreach (var ip in localIPs)
                    result.Add($"{scheme}://{ip}:{port}");
            }
            else
            {
                result.Add($"{scheme}://{host}:{port}");
            }
        }

        return [.. result];
    }

    static IEnumerable<string> GetLocalIPAddresses() =>
        from ni in NetworkInterface.GetAllNetworkInterfaces()
        where ni.OperationalStatus == OperationalStatus.Up
        from ua in ni.GetIPProperties().UnicastAddresses
        let ip = ua.Address
        where (ip.AddressFamily is AddressFamily.InterNetwork or AddressFamily.InterNetworkV6)
              && !IPAddress.IsLoopback(ip)
        select ip.AddressFamily == AddressFamily.InterNetworkV6 ? $"[{ip}]" : ip.ToString();
}
