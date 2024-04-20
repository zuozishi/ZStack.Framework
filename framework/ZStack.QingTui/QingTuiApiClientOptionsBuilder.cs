namespace ZStack.QingTui;

public class QingTuiApiClientOptionsBuilder
{
    private readonly QingTuiApiClientOptions _options = new();

    public static QingTuiApiClientOptionsBuilder Create()
        => new();

    public QingTuiApiClientOptionsBuilder SetDisplayName(string displayName)
    {
        _options.AppId = displayName;
        return this;
    }

    public QingTuiApiClientOptionsBuilder SetAppId(string appId)
    {
        _options.AppId = appId;
        return this;
    }

    public QingTuiApiClientOptionsBuilder SetSecret(string secret)
    {
        _options.Secret = secret;
        return this;
    }

    public QingTuiApiClientOptionsBuilder SetHost(string host)
    {
        _options.Host = host;
        return this;
    }

    public QingTuiApiClientOptionsBuilder SetTokenPersister(ITokenPersister tokenPersister)
    {
        _options.TokenPersister = tokenPersister;
        return this;
    }

    public QingTuiApiClientOptionsBuilder SetLogger(ILogger logger)
    {
        _options.Logger = logger;
        return this;
    }

    public QingTuiApiClientOptions Build()
    {
        if (string.IsNullOrEmpty(_options.AppId))
            throw new ArgumentException("AppId is required");
        if (string.IsNullOrEmpty(_options.Secret))
            throw new ArgumentException("Secret is required");
        if (string.IsNullOrEmpty(_options.Host))
            throw new ArgumentException("Host is required");
        _options.TokenPersister ??= new LocalTokenPersister();
        return _options;
    }
}
