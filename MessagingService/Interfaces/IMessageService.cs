using MessagingService.Models;

namespace MessagingService.Interfaces;

public interface IMessageService
{
    public Task<Message> SendMessageAsync(OutboundSmsMms message);
    public Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId, int? size, DateTime? cursor = null);
    public Task<Message> SendEmailAsync(OutboundEmail payload);
}
