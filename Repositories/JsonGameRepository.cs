using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

internal class JsonGameRepository : IGameRepository
{
    private readonly List<GameRecord> _games;
    private readonly JsonFileManager _fileManager;
    private readonly string _filePath;

    public JsonGameRepository(JsonFileManager fileManager, string filePath)
    {
        _fileManager = fileManager;
        _filePath = filePath;
        _games = _fileManager.LoadAll<GameRecord>(_filePath);
    }

    public List<GameRecord> GetAll() => _games;

    public void SaveGame(GameRecord game)
    {
        _games.Add(game);
        _fileManager.WriteAll(_filePath, _games);
    }

    public void Clear()
    {
        _games.Clear();
        _fileManager.WriteAll(_filePath, _games);
    }
}
