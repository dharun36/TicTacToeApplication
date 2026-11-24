
namespace TicTacToeApplication.Models;

/// <summary>
/// Represents a single move made during a game.
/// </summary>
internal class MoveRecord
{
    /// <summary>
    /// Gets or sets the selected cell (0 to 8).
    /// </summary>
    public int Cell { get; set; }

    /// <summary>
    /// Gets or sets the symbol placed in the cell.
    /// </summary>
    public Enums.Symbol Symbol { get; set; }

    /// <summary>
    /// Gets or sets the time at which the move was made.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
