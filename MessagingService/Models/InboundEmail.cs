using System.Text.Json.Serialization;
using MessagingService.Validation;
namespace MessagingService.Models;

public class InboundEmail
{
    [StrictEmail]
    public string From { get; set; }
    [StrictEmail]
    public string To { get; set; }
    [JsonPropertyName("xillio_id")]
    public string XillioId { get; set; }
    public string Body { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public List<string> Attachments { get; set; } = new List<string>();
}

