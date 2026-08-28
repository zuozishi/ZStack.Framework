using Microsoft.Extensions.Options;
using ZStack.AspNetCore.QingTui;

namespace ZStackWebApi.Examples;

/// <summary>
/// QingTuiComponent 示例：检查客户端配置。
/// </summary>
[ApiController]
[Route("api/examples/qingtui")]
public sealed class QingTuiExampleController(
    IOptions<QingTuiClientOptions> options,
    IQingTuiClientService clientService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var configuredClients = options.Value.Clients
            .Select(client => new { client.DisplayName, client.AppId })
            .ToArray();

        return Ok(new
        {
            options.Value.Host,
            ConfiguredClients = configuredClients,
            ClientServiceReady = clientService is not null,
            Hint = "将 Configuration/qingtui.json 中的 AppId 和 Secret 替换为真实配置后即可获取客户端。"
        });
    }
}
