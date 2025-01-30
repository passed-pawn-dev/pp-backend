using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class Knight(Color color) : Piece(color)
{
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhiteKnight : FenChar.BlackKnight;
}
