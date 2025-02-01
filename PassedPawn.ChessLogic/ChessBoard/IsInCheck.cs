using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool IsInCheck()
    {
        var (kingRow, kingCol) = FindKing();
        
        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                var piece = Board[row, col];
                if (piece is null || piece.Color == CurrentPlayer) continue;

                if (CanPieceMove(row, col, kingRow, kingCol))
                    return true;
            }
        }

        return false;
    }

    private Coords FindKing()
    {
        var expectedFenChar = CurrentPlayer == Color.White ? FenChar.WhiteKing : FenChar.BlackKing;
        
        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                var piece = Board[row, col];

                if (piece?.FenChar == expectedFenChar)
                    return new Coords(row, col);
            }
        }

        throw new Exception("No king");
    }
}
