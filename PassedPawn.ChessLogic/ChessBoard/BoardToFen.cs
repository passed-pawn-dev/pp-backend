using System.Text;
using PassedPawn.ChessLogic.Models;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    public string BoardToFen()
    {
        var fen = new StringBuilder();
    
        for (var row = BoardSize - 1; row >= 0; row--)
        {
            var emptyInARow = 0;

            for (var col = 0; col < BoardSize; col++)
            {
                var piece = Board[row, col];

                if (piece is null)
                {
                    emptyInARow++;
                }
                else
                {
                    if (emptyInARow > 0)
                    {
                        fen.Append(emptyInARow);
                        emptyInARow = 0;
                    }

                    fen.Append((char)piece.FenChar);
                }
            }

            if (emptyInARow > 0)
                fen.Append(emptyInARow);

            if (row > 0)
                fen.Append('/');
        }

        fen.Append(' ');
        fen.Append(CurrentPlayer == Color.White ? "w " : "b ");

        var castlingRights = false;

        if (_canWhiteCastleKingSide)
        {
            fen.Append('K');
            castlingRights = true;
        }
        
        if (_canWhiteCastleQueenSide)
        {
            fen.Append('Q');
            castlingRights = true;
        }
        
        if (_canBlackCastleKingSide)
        {
            fen.Append('k');
            castlingRights = true;
        }
        
        if (_canBlackCastleQueenSide)
        {
            fen.Append('q');
            castlingRights = true;
        }

        if (!castlingRights)
            fen.Append('-');

        return fen.ToString();
    }
}
