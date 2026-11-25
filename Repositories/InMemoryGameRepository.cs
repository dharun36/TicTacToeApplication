using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

internal class InMemoryGameRepository : IGameRepository
{
    private readonly List<GameRecord> _games = new();

    public List<GameRecord> GetAll() => _games;

    public void SaveGame(GameRecord game)
    {
        _games.Add(game);
    }

    public void Clear()
    {
        _games.Clear();
    }
}
