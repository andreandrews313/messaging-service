using MessagingService.Models;
namespace MessagingService.Providers;
public class MockEmailProvider : IMessageProvider
{
    private readonly ILogger<MockEmailProvider> _logger;

    public MockEmailProvider(ILogger<MockEmailProvider> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendMessageAsync(Message message)
    {
        _logger.LogInformation($"Sending email to {message.To} via Email Provider");
        await Task.Delay(100); 
        return true;
    }

}
