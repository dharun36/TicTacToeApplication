
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Services.Strategies;

/// <summary>
/// Provides the Minimax algorithm for selecting optimal moves.
/// </summary>
internal class MinimaxAlgorithm
{
    private readonly TicTacToeService _gameService;

    public MinimaxAlgorithm(TicTacToeService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Finds the best possible move for the computer.
    /// </summary>
    public int GetBestMove(Symbol[] board, Symbol computerSymbol)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (board.Length != 9)
        {
            throw new ArgumentException(
                "The board must contain exactly 9 cells.",
                nameof(board));
        }

        List<int> availableCells = GetAvailableCells(board);

        if (availableCells.Count == 0)
        {
            throw new InvalidOperationException(
                "No available cells remain.");
        }

        int bestScore = int.MinValue;
        int bestMove = -1;

        Symbol opponentSymbol = GetOpponent(computerSymbol);

        foreach (int cell in availableCells)
        {
            board[cell] = computerSymbol;

            int score;

            try
            {
                score = Minimax(
                    board,
                    opponentSymbol,
                    computerSymbol,
                    1);
            }
            finally
            {
                board[cell] = Symbol.Empty;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = cell;
            }
        }

        return bestMove;
    }

    private int Minimax(
        Symbol[] board,
        Symbol currentSymbol,
        Symbol computerSymbol,
        int depth)
    {
        Symbol winner = _gameService.IsWinCondition(board);

        if (winner == computerSymbol)
        {
            return 10 - depth;
        }

        if (winner != Symbol.Empty)
        {
            return depth - 10;
        }

        if (_gameService.IsDraw(board))
        {
            return 0;
        }

        List<int> availableCells = GetAvailableCells(board);

        Symbol nextSymbol = GetOpponent(currentSymbol);

        if (currentSymbol == computerSymbol)
        {
            int bestScore = int.MinValue;

            foreach (int cell in availableCells)
            {
                board[cell] = currentSymbol;

                int score;

                try
                {
                    score = Minimax(
                        board,
                        nextSymbol,
                        computerSymbol,
                        depth + 1);
                }
                finally
                {
                    board[cell] = Symbol.Empty;
                }

                bestScore = Math.Max(bestScore, score);
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            foreach (int cell in availableCells)
            {
                board[cell] = currentSymbol;

                int score;

                try
                {
                    score = Minimax(
                        board,
                        nextSymbol,
                        computerSymbol,
                        depth + 1);
                }
                finally
                {
                    board[cell] = Symbol.Empty;
                }

                bestScore = Math.Min(bestScore, score);
            }

            return bestScore;
        }
    }

    private static List<int> GetAvailableCells(Symbol[] board)
    {
        return Enumerable.Range(0, board.Length)
            .Where(index => board[index] == Symbol.Empty)
            .ToList();
    }

    private static Symbol GetOpponent(Symbol symbol)
    {
        return symbol == Symbol.X ? Symbol.O : Symbol.X;
    }
}
