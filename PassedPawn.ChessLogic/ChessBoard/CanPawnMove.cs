using PassedPawn.ChessLogic.Models;

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
                     Board[newRow, newCol - 1] is null,
                -2 => !hasMoved && CurrentPlayer == Color.Black && Board[newRow, newCol] is null &&
                      Board[newRow, newCol + 1] is null,
                _ => false
            };
            
            return canMove && IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
        }

        if (Math.Abs(rowDistance) != 1 || Math.Abs(newCol - prevCol) != 1)
            return false;

        var newPiece = Board[newRow, newCol];
        return (newPiece is null || newPiece.Color != CurrentPlayer)
               && IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
    }
}
