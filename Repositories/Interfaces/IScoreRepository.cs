using TicTacToeApplication.Models;

namespace TicTacToeApplication.Repositories.Interfaces;

internal interface IScoreRepository
{
    void SaveScore(Score score);

    Score GetScore(string username);

    List<Score> GetAll();
}
