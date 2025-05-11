using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PassedPawn.Models.Validators;

public partial class FenValidationAttribute : ValidationAttribute
{
    private static readonly Regex FenRegex = MyRegex();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;
        
        if (value is not string fen || string.IsNullOrWhiteSpace(fen))
            return new ValidationResult("FEN string cannot be empty.");

        if (!FenRegex.IsMatch(fen))
            return new ValidationResult("FEN string is not in a valid format.");

        var parts = fen.Split(' ');

        var rows = parts[0].Split('/');
        if (rows.Length != 8)
            return new ValidationResult("FEN must have exactly 8 rows.");

        foreach (var row in rows)
        {
            var count = 0;
            foreach (var c in row)
                count += char.IsDigit(c) ? c - '0' : 1;
            if (count != 8)
                return new ValidationResult("Each FEN row must contain exactly 8 squares.");
        }

        return ValidationResult.Success;
    }

    [GeneratedRegex(@"^([rnbqkpRNBQKP1-8]{1,8}/){7}[rnbqkpRNBQKP1-8]{1,8} (w|b) (-|K?Q?k?q?) (-|[a-h][36]) \d+ \d+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
