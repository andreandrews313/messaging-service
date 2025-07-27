using MessagingService.Models;

public interface IMessageProvider
{
    Task<bool> SendMessageAsync(Message message);
}
