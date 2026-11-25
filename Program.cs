using TicTacToeApplication.Controllers;
using TicTacToeApplication.Repositories;
using TicTacToeApplication.Repositories.Interfaces;
using TicTacToeApplication.Services;
using TicTacToeApplication.Views;

namespace TicTacToeApplication;

/// <summary>
/// Application entry point wires up the dependency once.
/// </summary>
internal class Program
{
    private static void Main()
    {
        JsonFileManager jsonFileManager = new JsonFileManager();

        IUserRepository userRepository = new JsonUserRepository(jsonFileManager, "User.json");
        AuthService authService = new AuthService(userRepository);
        AuthView authView = new AuthView();
        AuthController authController = new AuthController(authService, authView);

        JsonScoreRepository jsonScoreRepository = new JsonScoreRepository(jsonFileManager, "Score.json");
        InMemoryScoreRepository inMemoryScoreRepository = new InMemoryScoreRepository();

        IGameRepository jsonGameRepository = new JsonGameRepository(jsonFileManager, "GameHistory.json");
        IGameRepository inMemoryGameRepository = new InMemoryGameRepository();

        RepositorySelector repositorySelector = new RepositorySelector(
            jsonScoreRepository, inMemoryScoreRepository,
            jsonGameRepository, inMemoryGameRepository);

        ScoreService scoreService = new ScoreService(repositorySelector);

        BoardView boardView = new BoardView();
        TicTacToeService ticTacToeService = new TicTacToeService();
        GameController gameController = new GameController(boardView, ticTacToeService, scoreService, repositorySelector);

        AppView appView = new AppView();
        AppController appController = new AppController(authController, appView, gameController, scoreService);
        appController.Run();
    }
}