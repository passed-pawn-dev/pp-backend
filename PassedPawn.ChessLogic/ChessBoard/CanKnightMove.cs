using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanKnightMove(int prevRow, int prevCol, int newRow, int newCol)
    {
        IEnumerable<Coords> directions =
        [
            new Coords(1, 2), new Coords(1, -2), new Coords(-1, 2), new Coords(-1, -2),
            new Coords(2, 1), new Coords(2, -1), new Coords(-2, 1), new Coords(-2, -1)
        ];
        
        return !IsFriendlyPieceThere(newRow, newCol)
               && directions.Contains(new Coords(newRow - prevRow, newCol - prevCol));
    }
}
