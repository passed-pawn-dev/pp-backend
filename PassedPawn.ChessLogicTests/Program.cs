
using PassedPawn.ChessLogic;
using PassedPawn.ChessLogic.ChessBoard;
using PassedPawn.ChessLogic.Pieces;

public static class Program
{
    public static void Main(string[] args)
    {
        var chessBoard = new ChessBoard();
        Console.WriteLine(chessBoard.Move(1, 4, 3, 4));
        Console.WriteLine(chessBoard.Move(6, 0, 5, 0));
        Console.WriteLine(chessBoard.Move(3, 4, 4, 4));
        Console.WriteLine(chessBoard.Move(6, 3, 4, 3));
        Console.WriteLine(chessBoard.Move(4, 4, 5, 3));
        Console.WriteLine("-------------");
        Console.WriteLine(chessBoard.Board[5, 3] is Pawn);
        Console.WriteLine(chessBoard.Board[4, 3] is not Pawn);
    }
}