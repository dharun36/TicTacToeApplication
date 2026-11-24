
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Services.Strategies;

/// <summary>
/// Combines random selection and Minimax for medium difficulty.
/// </summary>
internal class MediumStrategy : IMoveStrategy
{
    private readonly MinimaxAlgorithm _minimaxAlgorithm;

    private const int MinimaxProbability = 50;

    public MediumStrategy(TicTacToeService gameService)
    {
        _minimaxAlgorithm = new MinimaxAlgorithm(gameService);
    }

    /// <summary>
    /// Selects a move using either random selection or Minimax.
    /// </summary>
    public int GetNextMove(Symbol[] board, Symbol computerSymbol)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (board.Length != 9)
        {
            throw new ArgumentException(
                "The board must contain exactly 9 cells.",
                nameof(board));
        }

        List<int> availableCells = Enumerable.Range(0, board.Length)
            .Where(index => board[index] == Symbol.Empty)
            .ToList();

        if (availableCells.Count == 0)
        {
            throw new InvalidOperationException(
                "No available cells remain.");
        }

        bool useMinimax =
            Random.Shared.Next(100) < MinimaxProbability;

        if (useMinimax)
        {
            return _minimaxAlgorithm.GetBestMove(
                board,
                computerSymbol);
        }

        int randomIndex = Random.Shared.Next(availableCells.Count);

        return availableCells[randomIndex];
    }
}
