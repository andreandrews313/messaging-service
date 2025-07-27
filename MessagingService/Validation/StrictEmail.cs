using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MessagingService.Validation;

public class StrictEmailAttribute : ValidationAttribute
{
    private static readonly Regex _strictEmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;
        if (string.IsNullOrWhiteSpace(email) || !_strictEmailRegex.IsMatch(email))
        {
            return new ValidationResult($"'{email}' is not a valid email address.");
        }

        return ValidationResult.Success;
    }
}
