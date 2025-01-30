using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanKingMove(bool hasMoved, int prevRow, int prevCol, int newRow, int newCol)
    {
        IEnumerable<Coords> directions = [
            new Coords(0, 2), new Coords(0, 2),
            new Coords(0, 1), new Coords(0, -1), new Coords(1, 0), new Coords(1, -1),
            new Coords(1, 1), new Coords(-1, 0), new Coords(-1, 1), new Coords(-1, -1)
        ];
        
        if (!hasMoved || RookHasNotMoved())
        {
            return NoPieceOnTheWay()
                   && directions.Contains(new Coords(newRow - prevRow, newCol - prevCol));
        }
        
        return !IsFriendlyPieceThere(newRow, newCol)
               && directions.Skip(2).Contains(new Coords(newRow - prevRow, newCol - prevCol));
        
        bool RookHasNotMoved()
        {
            var piece = newCol > prevCol ? Board[prevRow, 7] : Board[prevRow, 0];
            return piece is Rook { HasMoved: false };
        }

        bool NoPieceOnTheWay()
        {
            var piece1 = newCol > prevCol ? Board[prevRow, 5] : Board[prevRow, 3];
            var piece2 = newCol > prevCol ? Board[prevRow, 6] : Board[prevRow, 2];
            return piece1 is null && piece2 is null;
        }
    }
}
