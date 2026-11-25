namespace TicTacToeApplication.Models.Enums;

/// <summary>
/// Contains the timer option to start a new game.
/// </summary>
internal enum TimerOption
{
    /// <summary>
    /// Represents a option to start a game without timer.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents a option to start a game with 5 second timer.
    /// </summary>
    FiveSecond = 5,

    /// <summary>
    /// Represents a option to start a game with 10 second timer.
    /// </summary>
    TenSecond = 10,
}
