using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Services;

/// <summary>
/// Contains the business logic for service layer.
/// </summary>
internal class AuthService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userRepository">The instance of the repository.</param>
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Checks the unique username of the player, hashes the password and creates an instance.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <param name="name">The name of the player.</param>
    /// <param name="password">The password entered by the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <returns>A tuple with status and message returned from the method.</returns>
    public (bool success, string message) CreateUser(string userName, string name, string password, string email)
    {
        if (_userRepository.Exists(userName))
        {
            return (false, "Username already exists\n");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        User user = new User(userName, name, email, hashedPassword, DateTime.UtcNow);

        _userRepository.AddUser(user);
        return (true, "User created successfully\n");
    }

    /// <summary>
    /// Validates the credentials and creates a user session.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <param name="password">The password entered by the user.</param>
    /// <returns>The user session created.</returns>
    public UserSession? LogIn(string userName, string password)
    {
        User? user = _userRepository.GetUser(userName);

        if (user == null)
        {
            return null;
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);

        if (!isValid)
        {
            return null;
        }

        return new UserSession(user.UserName, false);
    }

    /// <summary>
    /// Creates a guest session.
    /// </summary>
    /// <returns>A guest session created.</returns>
    public UserSession LogInGuest()
    {
        return new UserSession("Guest", true);
    }
}