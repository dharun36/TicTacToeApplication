namespace TicTacToeApplication.Models;

/// <summary>
/// Represents a user session.
/// </summary>
internal class UserSession
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserSession"/> class.
    /// </summary>
    /// <param name="userName">The username of the logged in user.</param>
    /// <param name="isGuest">True if the user is guest</param>
    public UserSession(string userName, bool isGuest)
    {
        UserName = userName;
        IsGuest = isGuest;
        IsStart = true;
    }

    /// <summary>
    /// Gets or sets the username of the player.
    /// </summary>
    /// <value>The username of the player.</value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether user is guest or not.
    /// </summary>
    /// <value>The guest login.</value>
    public bool IsGuest { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has taken their last turn in the game.
    /// </summary>
    public bool IsStart { get; set; }
}
