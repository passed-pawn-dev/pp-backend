using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanPieceMove(int prevRow, int prevCol, int newRow, int newCol)
    {
        var piece = Board[prevRow, prevCol];

        if (piece is null)
            return false;

        return piece switch
        {
            Pawn pawn => CanPawnMove(pawn.HasMoved, prevRow, prevCol, newRow, newCol),
            Bishop => CanBishopMove(prevRow, prevCol, newRow, newCol),
            Knight => CanKnightMove(prevRow, prevCol, newRow, newCol),
            Rook => CanRookMove(prevRow, prevCol, newRow, newCol),
            Queen => CanQueenMove(prevRow, prevCol, newRow, newCol),
            King king => CanKingMove(prevRow, prevCol, newRow, newCol),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
