using MessagingService.Interfaces;
using MessagingService.Models;
using Microsoft.EntityFrameworkCore;
namespace MessagingService.Services;

public class WebHookService : IWebHookService
{
    private readonly AppDbContext _context;
    private readonly MessageProviderFactory _providerFactory;

    public WebHookService(AppDbContext context, MessageProviderFactory providerFactory)
    {
        _context = context;
        _providerFactory = providerFactory;
    }
    public async Task<Message> HandleInboundEmailAsync(InboundEmail request)
    {
        var convo = await GetOrCreateConversationAsync(request.From, request.To);


        var message = new Message
        {
            Id = Guid.NewGuid(),
            From = request.From,
            To = request.To,
            XillioId = request.XillioId,
            Body = request.Body,
            Type = "email",
            Timestamp = request.Timestamp,
            Attachments = request.Attachments,
            ConversationId = convo.Id,
        };
        var provider = _providerFactory.Resolve(message.Type);
        await provider.SendMessageAsync(message);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<Message> HandleInboundMessageAsync(InboundSmsMms request)
    {
        var convo = await GetOrCreateConversationAsync(request.From, request.To);

        var message = new Message
        {
            Id = Guid.NewGuid(),
            From = request.From,
            To = request.To,
            Type = request.Type,
            MessagingProviderId = request.MessagingProviderId ?? null,
            Body = request.Body,
            Attachments = request.Attachments ?? new List<string>(),
            Timestamp = request.Timestamp,
            ConversationId = convo.Id
        };

        var provider = _providerFactory.Resolve(request.Type);
        await provider.SendMessageAsync(message);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }


    private async Task<Conversation> GetOrCreateConversationAsync(string from, string to)
    {
        var key = GenerateParticipantKey(from, to);
        var convo = await _context.Conversations.FirstOrDefaultAsync(c =>
            (c.ParticipantA == from && c.ParticipantB == to) ||
            (c.ParticipantA == to && c.ParticipantB == from));


        if (convo != null) return convo;

        convo = new Conversation
        {
            ParticipantA = from,
            ParticipantB = to,
            ParticipantKey = key,
        };

        _context.Conversations.Add(convo);
        await _context.SaveChangesAsync();
        return convo;
    }

    private string GenerateParticipantKey(string a, string b)
    {
        return string.Compare(a, b, StringComparison.Ordinal) < 0
        ? $"{a}_{b}" : $"{b}_{a}";
    }
}
