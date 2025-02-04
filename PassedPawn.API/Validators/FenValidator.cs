using System.Text.RegularExpressions;

namespace PassedPawn.API.Validators;

public partial class FenValidator
{
    private static readonly Regex FenRegex = MyRegex();

    public static bool IsValidFen(string fen)
    {
        if (string.IsNullOrWhiteSpace(fen))
            return false;

        if (!FenRegex.IsMatch(fen))
            return false;

        var parts = fen.Split(' ');

        var rows = parts[0].Split('/');
        if (rows.Length != 8) return false;
        
        foreach (var row in rows)
        {
            var count = 0;
            foreach (var c in row)
                count += char.IsDigit(c) ? c - '0' : 1;
            if (count != 8) return false;
        }

        if (!int.TryParse(parts[4], out var halfmoveClock) || halfmoveClock < 0)
            return false;
        if (!int.TryParse(parts[5], out var fullmoveNumber) || fullmoveNumber < 1)
            return false;

        return true;
    }

    [GeneratedRegex(@"^([rnbqkpRNBQKP1-8]{1,8}/){7}[rnbqkpRNBQKP1-8]{1,8} (w|b) (-|K?Q?k?q?) (-|[a-h][36]) \d+ \d+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}