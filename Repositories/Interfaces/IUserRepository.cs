using TicTacToeApplication.Models;

namespace TicTacToeApplication.Repositories.Interfaces;

/// <summary>
/// Defines data access operations for managing user accounts.
/// </summary>
internal interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the repository
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    void AddUser(User user);

    /// <summary>
    /// Retrieves a user by their unique username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The matching <see cref="User"/> if found; otherwise, null.</returns>
    User? GetUser(string userName);

    /// <summary>
    /// Checks if a user with the specified username already exists.
    /// </summary>
    /// <param name="userName">The username to checked.</param>
    /// <returns>True if the user exists, otherwise false.</returns>
    bool Exists(string userName);
}