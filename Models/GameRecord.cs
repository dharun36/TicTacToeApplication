
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

internal class GameRecord
{
    public Guid GameId { get; set; } = Guid.NewGuid();

    public string PlayerOneName { get; set; } = string.Empty;
    public string PlayerTwoName { get; set; } = string.Empty;

    public Symbol PlayerOneSymbol { get; set; }
    public Symbol PlayerTwoSymbol { get; set; }

    public GameMode Mode { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.Now;

    public GameResult Result { get; set; }

    public string WinnerName { get; set; } = string.Empty;

    public TimerOption TimeLimitSeconds { get; set; }
    public EndReason EndReason { get; set; }

    public List<MoveRecord> Moves { get; set; } = new();
}
