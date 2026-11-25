using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Services.Strategies;

/// <summary>
/// Defines the contract for selecting a computer move.
/// </summary>
internal interface IMoveStrategy
{
    /// <summary>
    /// Selects the next available cell for the computer.
    /// </summary>
    /// <param name="board">Current board state.</param>
    /// <param name="computerSymbol">Symbol used by the computer.</param>
    /// <returns>Index of the selected cell (0 to 8).</returns>
    int GetNextMove(Symbol[] board, Symbol computerSymbol);
}