using PassedPawn.ChessLogic.Models;
using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.ChessBoard;

public partial class ChessBoard
{
    private bool CanKingMove(int prevRow, int prevCol, int newRow, int newCol)
    {
        if (Math.Abs(newCol - prevCol) == 2 && newRow == prevRow)
            return CanCastle(newCol > prevCol);
        
        IEnumerable<Coords> directions = [
            new Coords(0, 1), new Coords(0, -1), new Coords(1, 0), new Coords(1, -1),
            new Coords(1, 1), new Coords(-1, 0), new Coords(-1, 1), new Coords(-1, -1)
        ];
        
        return !IsFriendlyPieceThere(newRow, newCol)
               && directions.Contains(new Coords(newRow - prevRow, newCol - prevCol));

        bool CanCastle(bool kingSideCastle)
        {
            if (IsInCheck())
                return false;
            
            var rookPositionCol = kingSideCastle ? 7 : 0;
            var possibleRook = Board[prevRow, rookPositionCol];

            if (possibleRook is not Rook)
                return false;

            switch (rookPositionCol)
            {
                case 0 when CurrentPlayer == Color.White:
                    if (!_canWhiteCastleQueenSide) return false;
                    break;
                case 0 when CurrentPlayer == Color.Black:
                    if (!_canBlackCastleQueenSide) return false;
                    break;
                case 7 when CurrentPlayer == Color.White:
                    if (!_canWhiteCastleKingSide) return false;
                    break;
                case 7 when CurrentPlayer == Color.Black:
                    if (!_canBlackCastleKingSide) return false;
                    break;
            }

            var firstKingPositionCol = prevCol + (kingSideCastle ? 1 : -1);
            var secondKingPositionCol = prevCol + (kingSideCastle ? 2 : -2);

            if (Board[prevRow, firstKingPositionCol] is not null
                || Board[prevRow, secondKingPositionCol] is not null)
                return false;

            if (!kingSideCastle && Board[prevRow, 1] is not null)
                return false;

            return IsPositionSafeAfterMove(prevRow, prevCol,
                       firstKingPositionCol, firstKingPositionCol)
                   && IsPositionSafeAfterMove(prevRow, prevCol,
                       firstKingPositionCol, secondKingPositionCol);
        }
    }
}
