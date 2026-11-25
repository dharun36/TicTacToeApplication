using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Models;

/// <summary>
/// Represents the score and statistics of a registered player.
/// </summary>
internal class Score
{
    public Score(string username, int wins, int losses, int draws, int totalPoints)
    {
        this.Username = username;
        this.Wins = wins;
        this.Losses = losses;
        this.Draws = draws;
        this.TotalPoints = totalPoints;
    }

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

    public int TotalPoints { get; set; }
}