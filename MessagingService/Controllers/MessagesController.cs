using MessagingService.Interfaces;
using MessagingService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MessagingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{

    private readonly IMessageService _messageService;
    private AppDbContext _context;

    public MessagesController(IMessageService messageService, AppDbContext context)
    {
        _messageService = messageService;
        _context = context;
    }

    [HttpPost("sms")]
    public async Task<ActionResult<Message>> SendMessage([FromBody] OutboundSmsMms message)
    {
        var res = await _messageService.SendMessageAsync(message);
        return Ok(res);
    }

    [HttpPost("email")]
    public async Task<ActionResult<Message>> SendEmail([FromBody] OutboundEmail payload)
    {
        var email = await _messageService.SendEmailAsync(payload);
        return Ok(email);
    }
}   
