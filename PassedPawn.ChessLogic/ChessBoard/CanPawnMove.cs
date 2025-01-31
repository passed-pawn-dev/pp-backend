using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanPawnMove(bool hasMoved, int prevRow, int prevCol, int newRow, int newCol)
    {
        var rowDistance = newRow - prevRow;
        
        if (prevCol == newCol)
        {
            var canMove = rowDistance switch
            {
                1 => CurrentPlayer == Color.White && Board[newRow, newCol] is null,
                -1 => CurrentPlayer == Color.Black && Board[newRow, newCol] is null,
                2 => !hasMoved && CurrentPlayer == Color.White && Board[newRow, newCol] is null &&
                     Board[newRow - 1, newCol] is null,
                -2 => !hasMoved && CurrentPlayer == Color.Black && Board[newRow, newCol] is null &&
                      Board[newRow + 1, newCol] is null,
                _ => false
            };
            
            return canMove && IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
        }

        if (Math.Abs(rowDistance) != 1 || Math.Abs(newCol - prevCol) != 1)
            return false;

        if (CanCaptureEnPassant(prevRow, prevCol))
            return true;

        var newPiece = Board[newRow, newCol];
        return (newPiece is null || newPiece.Color != CurrentPlayer)
               && IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
        
        bool CanCaptureEnPassant(int pawnRow, int pawnCol)
        {
            if (_lastMove is null)
                return false;

            var (piece, lastPrevMove, _, lastNewRow, lastNewCol) = _lastMove;

            if (piece is not Pawn || Math.Abs(lastNewRow - lastPrevMove) != 2 || pawnRow != lastNewRow ||
                Math.Abs(pawnCol - lastNewCol) != 1)
                return false;

            var pawnNewPositionRow = pawnRow + (CurrentPlayer == Color.White ? 1 : -1);

            Board[lastNewRow, lastNewCol] = null;
            var isPositionSafe = IsPositionSafeAfterMove(pawnRow, pawnCol,
                pawnNewPositionRow, lastNewCol);
        
            Board[lastNewRow, lastNewCol] = piece;
            return isPositionSafe;
        }
    }
}
