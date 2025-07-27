using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Models;

public class Conversation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string ParticipantA { get; set; }
    [Required]
    public string ParticipantB { get; set; }
    public List<Message> Messages { get; set; }
    public string ParticipantKey { get; set; }
    public static string Normalize(string a, string b)
    {
        return string.Compare(a, b, StringComparison.Ordinal) <= 0 ? $"{a}_{b}" : $"{b}_{a}";
    }
}
