using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Services;

/// <summary>
/// Handles the business logic for the tic-tac-toe game.
/// </summary>
internal class TicTacToeService
{
    /// <summary>
    /// Checks if the match is the draw.
    /// </summary>
    /// <param name="moves">The current state of the game.</param>
    /// <returns>True if the match is draw, otherwise false.</returns>
    public bool IsDraw(Symbol[] moves)
    {
        return !moves.Any(x => x == Symbol.Empty);
    }

    /// <summary>
    /// Toggles the current symbol.
    /// </summary>
    /// <param name="currentSymbol">Current symbol to be toggled.</param>
    /// <returns>Toggled symbol.</returns>
    public Symbol ToggleSymbol(Symbol currentSymbol)
    {
        if (currentSymbol == Symbol.X)
        {
            return Symbol.O;
        }
        else
        {
            return Symbol.X;
        }
    }

    /// <summary>
    /// Check if there is any winner.
    /// </summary>
    /// <param name="moves">The state of the game.</param>
    /// <returns>The symbol of the winner who won, otherwise <see cref="Symbol.Empty"/>.</returns>
    public Symbol IsWinCondition(Symbol[] moves)
    {
        // Row
        if (moves[0] == moves[1] && moves[1] == moves[2] && moves[2] != Symbol.Empty)
        {
            return moves[0];
        }

        if (moves[3] == moves[4] && moves[4] == moves[5] && moves[5] != Symbol.Empty)
        {
            return moves[3];
        }

        if (moves[6] == moves[7] && moves[7] == moves[8] && moves[8] != Symbol.Empty)
        {
            return moves[6];
        }

        // Diagonals
        if (moves[0] == moves[4] && moves[8] == moves[4] && moves[8] != Symbol.Empty)
        {
            return moves[0];
        }

        if (moves[2] == moves[4] && moves[4] == moves[6] && moves[6] != Symbol.Empty)
        {
            return moves[2];
        }

        // Column
        if (moves[0] == moves[3] && moves[3] == moves[6] && moves[6] != Symbol.Empty)
        {
            return moves[0];
        }

        if (moves[1] == moves[4] && moves[7] == moves[4] && moves[7] != Symbol.Empty)
        {
            return moves[1];
        }

        if (moves[2] == moves[5] && moves[8] == moves[5] && moves[8] != Symbol.Empty)
        {
            return moves[2];
        }

        return Symbol.Empty;
    }
}
