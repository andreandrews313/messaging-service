using MessagingService.Models;
namespace MessagingService.Interfaces;

public interface IWebHookService
{
    public Task<Message> HandleInboundMessageAsync(InboundSmsMms request);
    public Task<Message> HandleInboundEmailAsync(InboundEmail request); 
}
