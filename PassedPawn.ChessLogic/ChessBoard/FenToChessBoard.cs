using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    public static ChessBoard FenToChessBoard(string fen)
    {
        var portions = fen.Split(' ');
        var rows = portions[0].Split('/');

        var board = new Piece?[8,8];

        for (var i = 0; i < BoardSize; i++)
        {
            var index = 0;

            foreach (var character in rows[i])
            {
                Piece? piece = character switch
                {
                    'r' => new Rook(Color.Black),
                    'n' => new Knight(Color.Black),
                    'b' => new Bishop(Color.Black),
                    'q' => new Queen(Color.Black),
                    'k' => new King(Color.Black),
                    'p' => new Pawn(Color.Black, i != 1),
                    'R' => new Rook(Color.White),
                    'N' => new Knight(Color.White),
                    'B' => new Bishop(Color.White),
                    'Q' => new Queen(Color.White),
                    'K' => new King(Color.White),
                    'P' => new Pawn(Color.White, i != 6),
                    _ => null
                };

                if (piece is null)
                {
                    index += int.Parse(character.ToString());
                }
                else
                {
                    board[BoardSize - 1 - i, index] = piece;
                    index++;
                }
            }
        }

        var currentPlayer = portions[1] switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new Exception("Invalid current player")
        };

        var canWhiteCastleKingSide = portions[2].Contains('K');
        var canWhiteCastleQueenSide = portions[2].Contains('Q');
        var canBlackCastleKingSide = portions[2].Contains('k');
        var canBlackCastleQueenSide = portions[2].Contains('q');

        return new ChessBoard(board, currentPlayer, canWhiteCastleKingSide, canWhiteCastleQueenSide,
            canBlackCastleKingSide, canBlackCastleQueenSide);
    }
}
