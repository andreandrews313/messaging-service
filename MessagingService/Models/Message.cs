// The Message object that will be saved to Postgres
using System.ComponentModel.DataAnnotations;
using MessagingService.Validation;

namespace MessagingService.Models;
public class Message
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    [Required]
    [RegularExpression("^(sms|mms|email)$", ErrorMessage = "Type must be one of: sms, mms, or email.")]
    public string Type { get; set; } 
    public string? XillioId { get; set; } 
    public string? MessagingProviderId { get; set; }
    public string Body { get; set; } = default!;
    public List<string>? Attachments { get; set; }
    [Required]
    public DateTimeOffset Timestamp { get; set; }
    [Required]
    public int ConversationId { get; set; }
}
