using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class Bishop(Color color) : Piece(color)
{
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhiteBishop : FenChar.BlackBishop;
}
