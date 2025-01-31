using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class King(Color color) : Piece(color)
{
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhiteKing : FenChar.BlackKing;
}