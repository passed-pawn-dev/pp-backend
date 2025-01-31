using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.Pieces;

public class Pawn(Color color, bool hasMoved = false) : Piece(color)
{
    public bool HasMoved { get; private set; } = hasMoved;
    public override FenChar FenChar { get; } = color == Color.White ? FenChar.WhitePawn : FenChar.BlackPawn;
    
    public void SetHasMoved()
    {
        HasMoved = true;
    }
}
