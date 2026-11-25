using TicTacToeApplication.Models;

namespace TicTacToeApplication.Repositories.Interfaces;

internal interface IGameRepository
{
    List<GameRecord> GetAll();
    void SaveGame(GameRecord game);
    void Clear();
}
