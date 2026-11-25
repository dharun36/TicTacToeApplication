using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Repositories;

namespace TicTacToeApplication.Services;

internal class ScoreService
{
    private readonly RepositorySelector _scoreRepository;

    public ScoreService(RepositorySelector repository)
    {
        _scoreRepository = repository;
    }

    public List<(string Username, int TotalPoints, int Wins)> GetScores(UserSession session)
    {
        return _scoreRepository.GetScoreRepository(session).GetAll()
            .OrderByDescending(s => s.TotalPoints)
            .ThenByDescending(s => s.Wins)
            .Take(20)
            .Select(s => (s.Username, s.TotalPoints, s.Wins))
            .ToList();
    }

    public Score GetScoreByUserName(string userName, UserSession session)
    {
        return _scoreRepository.GetScoreRepository(session).GetScore(userName);
    }

    public void RecordWin(string userName, GameMode mode, UserSession session)
    {
        Score playerScore = _scoreRepository.GetScoreRepository(session).GetScore(userName);
        int pointsEarned = GetPointsForMode(mode);

        playerScore.Wins++;
        playerScore.TotalPoints += pointsEarned;
        playerScore.PointsByMode[mode] = playerScore.PointsByMode.GetValueOrDefault(mode) + pointsEarned;

        _scoreRepository.GetScoreRepository(session).SaveScore(playerScore);
    }

    public void RecordLoss(string userName, UserSession session)
    {
        Score playerScore = _scoreRepository.GetScoreRepository(session).GetScore(userName);
        playerScore.Losses++;

        _scoreRepository.GetScoreRepository(session).SaveScore(playerScore);
    }

    public void RecordDraw(string userName, UserSession session)
    {
        Score playerScore = _scoreRepository.GetScoreRepository(session).GetScore(userName);
        playerScore.Draws++;

        _scoreRepository.GetScoreRepository(session).SaveScore(playerScore);
    }

    private int GetPointsForMode(GameMode mode)
    {
        return mode switch
        {
            GameMode.Easy => 1,
            GameMode.Medium => 2,
            GameMode.Hard => 3,
            GameMode.Manual => 2,
            _ => 0
        };
    }

    internal void ClearGuestData(UserSession session)
    {
        _scoreRepository.ClearGuest(session);
    }
}
