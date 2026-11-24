
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

/// <summary>
/// Represents a completed Tic Tac Toe game.
/// </summary>
internal class GameRecord
{
    public Guid GameId { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;

    public string Opponent { get; set; } = string.Empty;

    public GameMode Mode { get; set; }

    public DateTime PlayedAt { get; set; }

    public Symbol MyMark { get; set; }

    public Symbol FirstMark { get; set; }

    public GameResult Result { get; set; }

    public TimerOption TimeLimitSeconds { get; set; }

    public EndReason EndReason { get; set; }

    public List<MoveRecord> Moves { get; set; } = new();
}
