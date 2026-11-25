
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

internal class MoveRecord
{
    public int Cell { get; set; }

    public Symbol Symbol { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;

    public int RemainingSeconds { get; set; }
}
