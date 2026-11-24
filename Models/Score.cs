
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

/// <summary>
/// Represents the score and statistics of a registered player.
/// </summary>
internal class Score
{
    public string Username { get; set; }

    public Dictionary<GameMode, int> PointsByMode { get; set; } = new()
    {
        { GameMode.Easy, 0 },
        { GameMode.Medium, 0 },
        { GameMode.Hard, 0 },
        { GameMode.Manual, 0 }
    };

    public int Wins { get; set; }

    public int Losses { get; set; }

    public int Draws { get; set; }

    public int TotalPoints => PointsByMode.Values.Sum();
}
