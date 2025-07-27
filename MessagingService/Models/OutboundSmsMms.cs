using System.ComponentModel.DataAnnotations;
using MessagingService.Validation;
namespace MessagingService.Models;

public class OutboundSmsMms
{
    [Phone]
    public string From { get; set; }
    [Phone]
    public string To { get; set; }
    [RegularExpression("^(sms|mms|email)$", ErrorMessage = "Type must be one of: sms, mms, or email.")]
    public string Type { get; set; }
    public string Body { get; set; }
    public List<string>? Attachments { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
