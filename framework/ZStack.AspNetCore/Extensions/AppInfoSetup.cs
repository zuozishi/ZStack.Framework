using Microsoft.AspNetCore.Hosting.Server;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Microsoft.AspNetCore.Builder;

public static class AppInfoSetup
{
    /// <summary>
    /// 显示应用程序信息，包括监听的地址和端口
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder PrintAppEndpoints(this IApplicationBuilder app)
    {
        var serverAddressesFeature = app.ApplicationServices.GetRequiredService<IServer>().Features
            .Get<Hosting.Server.Features.IServerAddressesFeature>();
        app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<App>>();
            var addresses = GetListeningAddresses(serverAddressesFeature?.Addresses);
            if (addresses.Any())
                logger.LogInformation("Application listening on:\n{Urls}", string.Join("\n", addresses.Select(url => $"\t{url}")));
        });
        return app;
    }

    static IEnumerable<string> GetListeningAddresses(IEnumerable<string>? serverAddresses)
    {
        if (serverAddresses == null || !serverAddresses.Any())
            return [];

        var ipAddresses = GetLocalIPAddresses();
        var result = new List<string>();

        foreach (var address in serverAddresses)
        {
            if (Uri.TryCreate(address, UriKind.Absolute, out var uri))
            {
                var host = uri.Host;
                var port = uri.Port;
                var scheme = uri.Scheme;

                if (host is "*" or "+" or "0.0.0.0" or "[::]")
                {
                    // 处理通配符地址
                    result.Add($"{scheme}://localhost:{port}");
                    result.AddRange(ipAddresses.Select(ip => $"{scheme}://{ip}:{port}"));
                }
                else if (IPAddress.TryParse(host, out var ip))
                {
                    // 直接使用IP地址
                    result.Add($"{scheme}://{ip}:{port}");
                }
                else
                {
                    // 使用域名
                    result.Add($"{scheme}://{host}:{port}");
                }
            }
        }

        return result.Distinct();
    }

    static IEnumerable<string> GetLocalIPAddresses()
    {
        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()
                 .Where(n => n.OperationalStatus == OperationalStatus.Up))
        {
            foreach (var ip in ni.GetIPProperties().UnicastAddresses
                     .Select(a => a.Address))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork ||
                    ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (!IPAddress.IsLoopback(ip))
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                            yield return $"[{ip}]";
                        else
                            yield return ip.ToString();
                    }
                }
            }
        }
    }
}
