using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanCaptureEnPassant(Pawn pawn, int pawnRow, int pawnCol)
    {
        if (_lastMove is null)
            return false;

        var (piece, prevRow, prevCol, newRow, newCol) = _lastMove;

        if (piece is not Pawn || Math.Abs(newRow - prevRow) != 2 || pawnRow != newRow ||
            Math.Abs(pawnCol - newCol) != 1)
            return false;

        var pawnNewPositionRow = pawnRow + (CurrentPlayer == Color.White ? 1 : -1);
        var pawnNewPositionCol = newCol;

        Board[newRow, newCol] = null;
        var isPositionSafe = IsPositionSafeAfterMove(pawnRow, pawnCol,
            pawnNewPositionRow, pawnNewPositionCol);
        
        Board[newRow, newCol] = piece;
        return isPositionSafe;
    }
}