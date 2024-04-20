namespace ZStack.QingTui;

public class QingTuiClientOptions
{
    public string Host { get; set; } = "https://open.qingtui.com";

    public IReadOnlyList<ClientOptions> Clients { get; set; } = [];

    public class ClientOptions
    {
        public string DisplayName { get; set; } = string.Empty;

        public string AppId { get; set; } = string.Empty;

        public string Secret { get; set; } = string.Empty;
    }
}
