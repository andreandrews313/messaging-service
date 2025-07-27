using MessagingService.Interfaces;
using MessagingService.Models;
using Microsoft.EntityFrameworkCore;
namespace MessagingService.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;
    private readonly MessageProviderFactory _providerFactory;

    public MessageService(AppDbContext context, MessageProviderFactory providerFactory)
    {
        _context = context;
        _providerFactory = providerFactory;
    }

    public async Task<Message> SendMessageAsync(OutboundSmsMms request)
    {
        var provider = _providerFactory.Resolve(request.Type);
        var conversation = await _context.Conversations.FirstOrDefaultAsync(c =>
            (c.ParticipantA == request.From && c.ParticipantB == request.To) ||
            (c.ParticipantA == request.To && c.ParticipantB == request.From));

        if (conversation == null)
        {
            var key = GenerateParticipantKey(request.From, request.To);
            conversation = new Conversation
            {
                ParticipantA = request.From,
                ParticipantB = request.To,
                ParticipantKey = key
            };
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            From = request.From,
            To = request.To,
            Type = "email",
            Body = request.Body,
            Timestamp = request.Timestamp,
            Attachments = request.Attachments,
            MessagingProviderId = null,
            XillioId = null
        };

        message.ConversationId = conversation.Id;
        await provider.SendMessageAsync(message);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<List<Message>> GetMessagesByConversationIdAsync(int ConversationId, int? size = 15, DateTime? cursor = null)
    {
        var messages = await _context.Messages.Where(message => message.ConversationId == ConversationId)
        .OrderByDescending(message => message.Timestamp)
        .ToListAsync();

        if (cursor.HasValue)
        {
            messages = messages.Where(message => message.Timestamp < cursor).ToList();
        }

        return messages.Take(size.Value).ToList();
    }

    public async Task<Message> SendEmailAsync(OutboundEmail payload)
    {
        var conversation = await _context.Conversations.FirstOrDefaultAsync(c =>
            (c.ParticipantA == payload.From && c.ParticipantB == payload.To) ||
            (c.ParticipantA == payload.To && c.ParticipantB == payload.From));

        if (conversation == null)
        {
            var key = GenerateParticipantKey(payload.From, payload.To);
            conversation = new Conversation
            {
                ParticipantA = payload.From,
                ParticipantB = payload.To,
                ParticipantKey = key
            };
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        var msg = new Message
        {
            Id = Guid.NewGuid(),
            From = payload.From,
            To = payload.To,
            Type = "email",
            Body = payload.Body,
            Timestamp = payload.Timestamp,
            Attachments = payload.Attachments,
            ConversationId = conversation.Id
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();
        return msg;
    }


    private string GenerateParticipantKey(string a, string b)
    {
        return string.Compare(a, b, StringComparison.Ordinal) < 0
        ? $"{a}_{b}" : $"{b}_{a}";
    }
}
