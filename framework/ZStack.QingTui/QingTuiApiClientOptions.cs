namespace ZStack.QingTui;

public class QingTuiApiClientOptions
{
    public string DisplayName { get; internal set; } = string.Empty;

    public string AppId { get; internal set; } = string.Empty;

    public string Secret { get; internal set; } = string.Empty;

    public string Host { get; internal set; } = "https://open.qingtui.com";

    public ITokenPersister? TokenPersister { get; internal set; }

    public ILogger? Logger { get; internal set; }
}
