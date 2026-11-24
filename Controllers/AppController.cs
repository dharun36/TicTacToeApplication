using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Views;

namespace TicTacToeApplication.Controllers;

/// <summary>
/// Coordinates between the view and service.
/// </summary>
internal class AppController
{
    private readonly AuthController _authController;
    private readonly AppView _appView;
    private readonly GameController _gameController;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppController"/> class.
    /// </summary>
    /// <param name="authController">Instance of the authentication controller.</param>
    /// <param name="appView">Instance of the view.</param>
    /// <param name="gameController">Instance of the game controller.</param>
    public AppController(AuthController authController, AppView appView, GameController gameController)
    {
        _authController = authController;
        _appView = appView;
        _gameController = gameController;
    }

    /// <summary>
    /// Entry point of the application.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            UserSession? session = _authController.HandleAuth();
            if (session == null)
            {
                break;
            }

            RunMainMenu(session);
        }
    }

    private void RunMainMenu(UserSession session)
    {
        while (true)
        {
            MainMenuOption option = _appView.GetMenuOption(session);

            switch (option)
            {
                case MainMenuOption.NewGame:
                    StartNewGame(session);
                    break;

                case MainMenuOption.History:
                    break;

                case MainMenuOption.Profile:
                    break;

                case MainMenuOption.LeaderBoard:
                    break;

                case MainMenuOption.Logout:
                    return;
            }
        }
    }

    private void StartNewGame(UserSession session)
    {
        GameMode mode = _appView.GetGameMode();

        TimerOption timerOption = _appView.GetTimerOption();

        _gameController.StartGame(
            mode,
            timerOption,
            session.UserName);

        Ui.Pause();
    }
}
