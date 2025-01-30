using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    public Piece?[,] Board { get; } =
    {
        {
            new Rook(Color.White), new Knight(Color.White), new Bishop(Color.White), new Queen(Color.White),
            new King(Color.White), new Bishop(Color.White), new Knight(Color.White), new Rook(Color.White)
        },
        {
            new Pawn(Color.White), new Pawn(Color.White), new Pawn(Color.White), new Pawn(Color.White),
            new Pawn(Color.White), new Pawn(Color.White), new Pawn(Color.White), new Pawn(Color.White)
        },
        { null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null},
        {
            new Pawn(Color.Black), new Pawn(Color.Black), new Pawn(Color.Black), new Pawn(Color.Black),
            new Pawn(Color.Black), new Pawn(Color.Black), new Pawn(Color.Black), new Pawn(Color.Black)
        },
        {
            new Rook(Color.Black), new Knight(Color.Black), new Bishop(Color.Black), new Queen(Color.Black),
            new King(Color.Black), new Bishop(Color.Black), new Knight(Color.Black), new Rook(Color.Black)
        }
    };

    public Color CurrentPlayer { get; private set; } = Color.White;
    private const int BoardSize = 8;

    private static bool AreCoordsValid(int row, int col) => row is >= 0 and < 8 && col is >= 0 and < 8;

    private bool IsPositionSafeAfterMove(Piece piece, int prevRow, int prevCol, int newRow, int newCol)
    {
        Board[prevRow, prevCol] = null;
        Board[newRow, newCol] = piece;

        var isPositionSafe = IsInCheck();

        Board[prevRow, prevCol] = piece;
        Board[newRow, newCol] = null;

        return !isPositionSafe;
    }

    public bool Move(int prevRow, int prevCol, int newRow, int newCol)
    {
        if (!AreCoordsValid(prevRow, prevCol) || !CanPieceMove(prevRow, prevCol, newRow, newCol))
            return false;

        var piece = Board[prevRow, prevCol];
        
        if (piece is null || piece.Color != CurrentPlayer)
            return false;
        
        if (piece is King kingToCastle && Math.Abs(newCol - prevCol) == 2)
        {
            var kingSideCastling = newCol > prevCol;

            if (!CanCastle(kingToCastle, kingSideCastling))
                return false;
            
            var rookPositionCol = newCol > prevCol ? 7 : 0;
            var rook = Board[prevRow, rookPositionCol];
            var rookNewPositionCol = newCol > prevCol ? 5 : 3;
            Board[prevRow, rookPositionCol] = null;
            Board[prevRow, rookNewPositionCol] = rook;
            Board[prevRow, prevCol] = null;
            Board[newRow, newCol] = piece;
        }
        else
        {
            var newPiece = Board[newRow, newCol];
            if (newPiece is not null && newPiece.Color == piece.Color) return false;
        
            Board[prevRow, prevCol] = null;
            Board[newRow, newCol] = piece;

            if (IsInCheck())
            {
                Board[prevRow, prevCol] = piece;
                Board[newRow, newCol] = null;
                return false;
            }
        }
        
        switch (piece)
        {
            case Pawn pawn:
                pawn.SetHasMoved();
                break;
            case Rook rook:
                rook.SetHasMoved();
                break;
            case King king:
                king.SetHasMoved();
                break;
        }
        
        CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
        return true;
    }
}
