namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanBishopMove(int prevRow, int prevCol, int newRow, int newCol)
    {
        var distanceRow = newRow - prevRow;
        var distanceCol = newCol - prevCol;

        if (Math.Abs(distanceRow) != Math.Abs(distanceCol))
            return false;

        if (IsFriendlyPieceThere(newRow, newCol))
            return false;
        
        distanceRow += distanceRow > 0 ? -1 : 1;
        distanceCol += distanceCol > 0 ? -1 : 1;

        while (distanceRow != 0)
        {
            if (Board[prevRow + distanceRow, prevCol + distanceCol] is not null)
                return false;
            
            distanceRow += distanceRow > 0 ? -1 : 1;
            distanceCol += distanceCol > 0 ? -1 : 1;
        }

        return IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
    }
}
