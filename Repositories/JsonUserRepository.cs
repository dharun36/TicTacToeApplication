using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

/// <summary>
/// Provides a JSON file based implementation of <see cref="IUserRepository"/> for persisting and retrieving user accounts.
/// </summary>
internal class JsonUserRepository : IUserRepository
{
    private readonly List<User> _users;
    private readonly JsonFileManager _jsonFileManager;
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonUserRepository"/> class.
    /// </summary>
    /// <param name="jsonFileManager">The instance of the Json file manager</param>
    /// <param name="path">The path of the file to be stored.</param>
    public JsonUserRepository(JsonFileManager jsonFileManager, string path)
    {
        _jsonFileManager = jsonFileManager;
        _filePath = path;
        _users = _jsonFileManager.LoadAll<User>(_filePath);
    }

    /// <inheritdoc/>
    public void AddUser(User user)
    {
        _users.Add(user);
        _jsonFileManager.WriteAll(_filePath, _users);
    }

    /// <inheritdoc/>
    public User? GetUser(string userName)
    {
        return _users.FirstOrDefault(user => user.UserName == userName);
    }

    /// <inheritdoc/>
    public bool Exists(string userName)
    {
        return _users.Any(user => user.UserName == userName);
    }
}
