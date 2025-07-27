using MessagingService.Providers;

public class MessageProviderFactory
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly Dictionary<string, IMessageProvider>? _testProviders;

    public MessageProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Used ONLY for testing
    public MessageProviderFactory(Dictionary<string, IMessageProvider> testProviders)
    {
        _testProviders = testProviders;
    }

    public IMessageProvider Resolve(string type)
    {
        return type switch
        {
            "sms" or "mms" => GetProvider("sms"),
            "email" => GetProvider("email"),
            _ => throw new ArgumentException($"Unsupported message type: {type}")
        };
    }

    private IMessageProvider GetProvider(string key)
    {
        if (_testProviders != null && _testProviders.TryGetValue(key, out var testProvider))
            return testProvider;

        if (_serviceProvider != null)
        {
            return key switch
            {
                "sms" or "mms" => _serviceProvider.GetRequiredService<MockSmsProvider>(),
                "email" => _serviceProvider.GetRequiredService<MockEmailProvider>(),
                _ => throw new ArgumentException($"Unsupported message type: {key}")
            };
        }

        throw new InvalidOperationException("No provider source available.");
    }
}
