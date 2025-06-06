namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanRookMove(int prevRow, int prevCol, int newRow, int newCol)
    {
        var distanceRow = newRow - prevRow;
        var distanceCol = newCol - prevCol;

        if (Math.Abs(distanceRow - distanceCol) != Math.Abs(distanceRow + distanceCol))
            return false;

        if (IsFriendlyPieceThere(newRow, newCol))
            return false;

        if (distanceRow == 0)
        {
            distanceCol += distanceCol > 0 ? -1 : 1;

            while (distanceCol != 0)
            {
                if (Board[prevRow, prevCol + distanceCol] is not null)
                    return false;
            
                distanceCol += distanceCol > 0 ? -1 : 1;
            }

            return IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
        }
        
        distanceRow += distanceRow > 0 ? -1 : 1;

        while (distanceRow != 0)
        {
            if (Board[prevRow + distanceRow, prevCol] is not null)
                return false;
        
            distanceRow += distanceRow > 0 ? -1 : 1;
        }

        return IsPositionSafeAfterMove(prevRow, prevCol, newRow, newCol);
    }
}
