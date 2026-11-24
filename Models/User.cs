namespace TicTacToeApplication.Models;

/// <summary>
/// Represents a user.
/// </summary>
internal class User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <param name="name">The name of the payer.</param>
    /// <param name="email">The email of the player.</param>
    /// <param name="hashedPassword">The password of the players account.</param>
    /// <param name="joinDate">The date of creating account.</param>
    public User(string userName, string name, string email, string hashedPassword, DateTime joinDate)
    {
        UserName = userName;
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        JoinDate = joinDate;
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username of the user.
    /// </value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email of the player.
    /// </summary>
    /// <value> The email of the player.</value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the hashed password.
    /// </summary>
    /// <value> The hashed password.</value>
    public string HashedPassword { get; set; }

    /// <summary>
    /// Gets or sets the date on which the user is created.
    /// </summary>
    /// <value>Join date of the user.</value>
    public DateTime JoinDate { get; set; }
}
