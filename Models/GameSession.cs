
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

/// <summary>
/// Represents the current state of an active game.
/// This object exists only in memory.
/// </summary>
internal class GameSession
{
    public GameSession(
        GameMode mode,
        TimerOption timerOption,
        UserSession player1,
        UserSession? player2 = null)
    {
        Mode = mode;
        TimerOption = timerOption;
        Player1 = player1;
        Player2 = player2;

        Board = Enumerable.Repeat(Symbol.Empty, 9).ToArray();

        FirstMark = Symbol.X;
        CurrentTurn = FirstMark;

        Moves = new List<MoveRecord>();
        StartedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Selected game mode.
    /// </summary>
    public GameMode Mode { get; }

    /// <summary>
    /// First human player.
    /// </summary>
    public UserSession Player1 { get; }

    /// <summary>
    /// Second human player. Null when playing against the computer.
    /// </summary>
    public UserSession? Player2 { get; }

    /// <summary>
    /// Current state of the 3x3 board.
    /// </summary>
    public Symbol[] Board { get; }

    /// <summary>
    /// Symbol of the player who starts the game.
    /// </summary>
    public Symbol FirstMark { get; set; }

    /// <summary>
    /// Symbol of the player whose turn is currently active.
    /// </summary>
    public Symbol CurrentTurn { get; set; }

    /// <summary>
    /// Timer configuration for the game.
    /// </summary>
    public TimerOption TimerOption { get; }

    /// <summary>
    /// Stores all moves made during the current game.
    /// </summary>
    public List<MoveRecord> Moves { get; }

    /// <summary>
    /// Time at which the game started.
    /// </summary>
    public DateTime StartedAt { get; }
}
