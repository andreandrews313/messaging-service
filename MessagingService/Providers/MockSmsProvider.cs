using MessagingService.Models;
namespace MessagingService.Providers;
public class MockSmsProvider : IMessageProvider
{
    private readonly ILogger<MockSmsProvider> _logger;

    public MockSmsProvider(ILogger<MockSmsProvider> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendMessageAsync(Message message)
    {
        int attempts = 0;
        int maxRetries = 3;

        while (attempts < maxRetries)
        {
            attempts++;
            try
            {
                _logger.LogInformation($"Sending {message.Type} to {message.To} via SMS Provider [attempt {attempts}]");

                if (new Random().Next(0, 4) == 0)
                    throw new HttpRequestException("This is a Twilio 500");

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning($"Attempt {attempts} failed: {ex.Message}");
                await Task.Delay(500);
            }
        }

        _logger.LogError($"Failed to send {message.Type} to {message.To} after {maxRetries} retries.");
        return false;
    }
}
