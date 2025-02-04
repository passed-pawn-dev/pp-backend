using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.Validators;

namespace PassedPawn.Tests.Validators;

public class FenValidatorTests
{
    private readonly FenValidationAttribute _validator = new();

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1")]
    [InlineData("r1bqkbnr/pppppppp/n7/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 2 3")]
    public void ValidFen_ShouldPassValidation(string fen)
    {
        var result = _validator.GetValidationResult(fen, new ValidationContext(fen));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP w KQkq - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/9/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w X - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KX - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e9 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - -1 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0")]
    public void InvalidFen_ShouldFailValidation(string fen)
    {
        var result = _validator.GetValidationResult(fen, new ValidationContext(fen));
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.False(string.IsNullOrWhiteSpace(result?.ErrorMessage));
    }
}