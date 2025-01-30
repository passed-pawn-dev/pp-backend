using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class Rook(Color color) : Piece(color)
{
    public bool HasMoved { get; private set; }
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhiteRook : FenChar.BlackRook;
    
    public void SetHasMoved()
    {
        HasMoved = true;
    }
}
