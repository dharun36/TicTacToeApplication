using TicTacToeApplication.Models;
using TicTacToeApplication.Repositories.Interfaces;

namespace TicTacToeApplication.Repositories;

internal class RepositorySelector
{
    private readonly JsonScoreRepository _jsonScoreRepository;
    private readonly InMemoryScoreRepository _inMemoryScoreRepository;
    private readonly IGameRepository _jsonGameRepository;
    private readonly IGameRepository _inMemoryGameRepository;

    public RepositorySelector(JsonScoreRepository jsonScoreRepository,
        InMemoryScoreRepository inMemoryScoreRepository,
        IGameRepository jsonGameRepository,
        IGameRepository inMemoryGameRepository)
    {
        _jsonScoreRepository = jsonScoreRepository;
        _inMemoryScoreRepository = inMemoryScoreRepository;
        _jsonGameRepository = jsonGameRepository;
        _inMemoryGameRepository = inMemoryGameRepository;
    }

    public IScoreRepository GetScoreRepository(UserSession session) =>
        session.IsGuest ? _inMemoryScoreRepository : _jsonScoreRepository;

    public IGameRepository GetGameRepository(UserSession session) =>
        session.IsGuest ? _inMemoryGameRepository : _jsonGameRepository;

    internal void ClearGuest(UserSession session)
    {
        if (!session.IsGuest) return;
        _inMemoryScoreRepository.Remove();
        _inMemoryGameRepository.Clear();
    }
}
