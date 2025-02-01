using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public abstract class Piece(Color color)
{
    public Color Color { get; } = color;
    public abstract FenChar FenChar { get; }
}
