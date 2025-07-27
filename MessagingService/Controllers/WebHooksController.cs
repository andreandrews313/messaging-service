using System.Windows.Markup;
using MessagingService.Interfaces;
using MessagingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WebHooksController : ControllerBase
{
    public IWebHookService _webhookService;
    public WebHooksController(IWebHookService webhookService)
    {
        _webhookService = webhookService;
    }

    [HttpPost("sms")]
    [HttpPost("mms")]
    public async Task<IActionResult> ReceiveSms([FromBody] InboundSmsMms message)
    {
        var msg = await _webhookService.HandleInboundMessageAsync(message);
        return Ok(msg);
    }

    [HttpPost("email")]
    public async Task<IActionResult> ReceiveEmail([FromBody] InboundEmail message)
    {
        var msg = await _webhookService.HandleInboundEmailAsync(message);
        return Ok(msg);
    }
}
