using System.ComponentModel.DataAnnotations;

namespace MessagingService.Models;


public class MessageBase
{
    public Guid Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Body { get; set; }
    [RegularExpression("^(sms|mms|email)$", ErrorMessage = "Type must be one of: sms, mms, or email.")]
    public string Type { get; set; }
    public string ProviderMessageId { get; set; }
}
