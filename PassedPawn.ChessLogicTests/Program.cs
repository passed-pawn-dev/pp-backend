
using PassedPawn.ChessLogic;
using PassedPawn.ChessLogic.ChessBoard;

public static class Program
{
    public static void Main(string[] args)
    {
        var chessBoard = new ChessBoard();
        Console.WriteLine(chessBoard.Move(1, 4, 3, 4));
        Console.WriteLine(chessBoard.Move(7, 6, 5, 5));
        Console.WriteLine(chessBoard.Move(0, 5, 4, 1));
        Console.WriteLine(chessBoard.Move(5, 5, 3, 6));
        Console.WriteLine(chessBoard.Move(0, 6, 2, 7));
        Console.WriteLine(chessBoard.Move(3, 6, 2, 4));
        Console.WriteLine(chessBoard.Move(0, 4, 0, 6));
        Console.WriteLine("Old king:");
        Console.WriteLine(chessBoard.Board[0, 4]);
        Console.WriteLine("New king:");
        Console.WriteLine(chessBoard.Board[0, 6]);
        Console.WriteLine("New Rook:");
        Console.WriteLine(chessBoard.Board[0, 5]);
        Console.WriteLine("Old Rook:");
        Console.WriteLine(chessBoard.Board[0, 7]);
        Console.WriteLine("Knight:");
        Console.WriteLine(chessBoard.Board[2, 5]);
    }
}