using MessagingService.Interfaces;
using MessagingService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IMessageService _messageService;
    private AppDbContext _context;

    public ConversationsController(IMessageService messageService, AppDbContext context)
    {
        _messageService = messageService;
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<Message>>> GetConversations()
    {
        var convos = await _context.Conversations.Include(c => c.Messages)
        .ToListAsync();

        var result = convos.Select(c => new
        {
            c.Id,
            c.ParticipantKey,
            c.ParticipantA,
            c.ParticipantB
        });

        return Ok(result);
    }

    [HttpGet("{conversationId}/messages")]
    public async Task<ActionResult<List<Message>>> GetMessagesByConversation([FromRoute] int conversationId, [FromQuery] DateTime? cursor = null, [FromQuery] int size = 15)
    {
        var messages = await _messageService.GetMessagesByConversationIdAsync(conversationId, size, cursor);

        if (messages.Count == 0)
        {
            return NotFound("No message found for conversation id!");
        }

        return Ok(messages);
    }
}
