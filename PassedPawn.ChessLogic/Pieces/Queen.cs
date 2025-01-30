using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class Queen(Color color) : Piece(color)
{
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhiteQueen : FenChar.BlackQueen;
}
