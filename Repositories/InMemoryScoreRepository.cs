using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

internal class InMemoryScoreRepository : IScoreRepository
{
    private readonly List<Score> _scores = new List<Score>();

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
    }

    public Score GetScore(string username)
    {
        return _scores.FirstOrDefault(s => s.Username == username) ?? new Score(username, 0, 0, 0, 0);
    }

    internal void Remove()
    {
        _scores.Clear();
    }
}
