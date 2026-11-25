using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Services;
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
    private readonly ScoreService _scoreService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppController"/> class.
    /// </summary>
    /// <param name="authController">Instance of the authentication controller.</param>
    /// <param name="appView">Instance of the view.</param>
    /// <param name="gameController">Instance of the game controller.</param>
    public AppController(AuthController authController, AppView appView, GameController gameController, ScoreService scoreService)
    {
        _authController = authController;
        _appView = appView;
        _gameController = gameController;
        _scoreService = scoreService;
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
            Score score = _scoreService.GetScoreByUserName(session.UserName, session);
            MainMenuOption option = _appView.GetMenuOption(session, score.TotalPoints, score.Wins);

            switch (option)
            {
                case MainMenuOption.NewGame:
                    StartNewGame(session);
                    break;

                case MainMenuOption.History:
                    this._gameController.ShowHistory(session);
                    break;

                case MainMenuOption.Profile:
                    DisplayProfile(session);
                    break;

                case MainMenuOption.LeaderBoard:
                    if(session.IsGuest)
                    {
                        this._appView.DisplayInfo("Login as user to the see the leaderboard\n");
                        break;
                    }

                    DisplayLeaderBoard(session);
                    break;

                case MainMenuOption.Logout:
                    if (session.IsGuest && this._appView.ConfirmLogout())
                    {
                        _scoreService.ClearGuestData(session);
                        return;
                    }

                    break;
            }

            Ui.Pause();
        }
    }
    
    private void DisplayProfile(UserSession session)
    {
        Score score = this._scoreService.GetScoreByUserName(session.UserName, session);
        _appView.DisplayProfile(score);
    }

    private void DisplayLeaderBoard(UserSession session)
    {
        List<(string Username, int TotalPoints, int Wins)> leaderboard = _scoreService.GetScores(session);
        _appView.DisplayLeaderBoard(session.UserName, leaderboard);
    }

    private void StartNewGame(UserSession session)
    {
        GameMode mode = _appView.GetGameMode();
        TimerOption timerOption = _appView.GetTimerOption();

        _gameController.StartGame(mode, timerOption, session);
    }
}
