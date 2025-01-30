using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanCastle(King king, bool kingSideCastle)
    {
        if (king.HasMoved)
            return false;

        var kingPositionRow = king.Color == Color.White ? 0 : 7;
        const int kingPositionCol = 4;
        var rookPositionCol = kingSideCastle ? 7 : 0;
        var possibleRook = Board[kingPositionRow, rookPositionCol];

        if (possibleRook is not Rook rook || rook.HasMoved || IsInCheck())
            return false;

        var firstKingPositionCol = kingPositionCol + (kingSideCastle ? 1 : -1);
        var secondKingPositionCol = kingPositionCol + (kingSideCastle ? 2 : -2);

        if (Board[kingPositionRow, firstKingPositionCol] is not null
            || Board[kingPositionRow, secondKingPositionCol] is not null)
            return false;

        if (!kingSideCastle && Board[kingPositionRow, 1] is not null)
            return false;

        return IsPositionSafeAfterMove(king, kingPositionRow, kingPositionCol,
                   kingPositionRow, firstKingPositionCol)
               && IsPositionSafeAfterMove(king, kingPositionRow, kingPositionCol,
                   kingPositionRow, secondKingPositionCol);
    }
}
