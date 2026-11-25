using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

internal class JsonScoreRepository : IScoreRepository
{
    private readonly List<Score> _scores;
    private readonly JsonFileManager _jsonFileManager;
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonUserRepository"/> class.
    /// </summary>
    /// <param name="jsonFileManager">The instance of the Json file manager</param>
    /// <param name="path">The path of the file to be stored.</param>
    public JsonScoreRepository(JsonFileManager jsonFileManager, string path)
    {
        _jsonFileManager = jsonFileManager;
        _filePath = path;
        _scores = _jsonFileManager.LoadAll<Score>(_filePath);
    }

    public List<Score> GetAll()
    {
        return _scores;
    }

    public void SaveScore(Score score)
    {
        int existingIndex = _scores.FindIndex(s => s.Username == score.Username);

        if (existingIndex != -1)
        {
            _scores[existingIndex] = score;
        }
        else
        {
            _scores.Add(score);
        }

        _jsonFileManager.WriteAll(_filePath, _scores);
    }

    public Score GetScore(string username)
    {
        return _scores.FirstOrDefault(s => s.Username == username) ?? new Score(username, 0, 0, 0, 0);
    }
}
