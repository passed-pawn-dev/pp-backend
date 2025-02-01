namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool IsFriendlyPieceThere(int newRow, int newCol)
    {
        var newPiece = Board[newRow, newCol];
        return newPiece is not null && newPiece.Color == CurrentPlayer;
    }
}
