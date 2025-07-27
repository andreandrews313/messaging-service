using MessagingService.Validation;
namespace MessagingService.Models;

public class OutboundEmail
{
    [StrictEmail]
    public string From { get; set; }
    [StrictEmail]
    public string To { get; set; }
    public string Body { get; set; }
    public List<string>? Attachments { get; set; } = new List<string>();
    public DateTimeOffset Timestamp { get; set; }
}
