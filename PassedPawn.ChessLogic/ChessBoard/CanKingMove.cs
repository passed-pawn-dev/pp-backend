using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanKingMove(bool hasMoved, int prevRow, int prevCol, int newRow, int newCol)
    {
        if (Math.Abs(newCol - prevCol) == 2 && newRow == prevRow)
            return CanCastle(newCol > prevCol);
        
        IEnumerable<Coords> directions = [
            new Coords(0, 1), new Coords(0, -1), new Coords(1, 0), new Coords(1, -1),
            new Coords(1, 1), new Coords(-1, 0), new Coords(-1, 1), new Coords(-1, -1)
        ];
        
        return !IsFriendlyPieceThere(newRow, newCol)
               && directions.Contains(new Coords(newRow - prevRow, newCol - prevCol));

        bool CanCastle(bool kingSideCastle)
        {
            if (hasMoved || IsInCheck())
                return false;
            
            var rookPositionCol = kingSideCastle ? 7 : 0;
            var possibleRook = Board[prevRow, rookPositionCol];

            if (possibleRook is not Rook rook || rook.HasMoved)
                return false;

            var firstKingPositionCol = prevCol + (kingSideCastle ? 1 : -1);
            var secondKingPositionCol = prevCol + (kingSideCastle ? 2 : -2);

            if (Board[prevRow, firstKingPositionCol] is not null
                || Board[prevRow, secondKingPositionCol] is not null)
                return false;

            if (!kingSideCastle && Board[prevRow, 1] is not null)
                return false;

            return IsPositionSafeAfterMove(prevRow, prevCol,
                       firstKingPositionCol, firstKingPositionCol)
                   && IsPositionSafeAfterMove(prevRow, prevCol,
                       firstKingPositionCol, secondKingPositionCol);
        }
    }
}
