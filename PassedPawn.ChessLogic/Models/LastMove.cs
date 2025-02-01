using PassedPawn.ChessLogic.Pieces;

namespace PassedPawn.ChessLogic.Models;

public record LastMove(Piece Piece, int PrevRow, int PrevCol, int NewRow, int NewCol);
