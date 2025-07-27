using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace MessagingService.Models;

public class InboundSmsMms
{
    public string From { get; set; }
    public string To { get; set; }
    public string Body { get; set; }
    [RegularExpression("^(sms|mms|email)$", ErrorMessage = "Type must be one of: sms, mms, or email.")]
    public string Type { get; set; }
    [JsonPropertyName("messaging_provider_id")]
    public string MessagingProviderId { get; set; }
    public List<string>? Attachments { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
