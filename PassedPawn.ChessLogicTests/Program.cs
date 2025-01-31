using PassedPawn.ChessLogic.ChessBoard;

public static class Program
{
    public static void Main(string[] args)
    {
        var chessBoard = new ChessBoard();
        Console.WriteLine(chessBoard.Move(1, 7, 3, 7));
        Console.WriteLine(chessBoard.Move(6, 0, 5, 0));
        Console.WriteLine(chessBoard.Move(0, 7, 2, 7));
        Console.WriteLine("-------------");
        var fen = chessBoard.BoardToFen();
        Console.WriteLine(fen);
        var board = ChessBoard.FenToChessBoard(fen);
        Console.WriteLine(board.BoardToFen());
    }
}