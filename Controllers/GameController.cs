
using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Repositories;
using TicTacToeApplication.Services;
using TicTacToeApplication.Services.Strategies;
using TicTacToeApplication.Views;

namespace TicTacToeApplication.Controllers;

internal class GameController
{
    private readonly BoardView _boardView;
    private readonly TicTacToeService _gameService;
    private readonly EasyStrategy _easyStrategy = new();
    private readonly MediumStrategy _mediumStrategy = new();
    private readonly HardStrategy _hardStrategy = new();
    private readonly ScoreService _scoreService;
    private readonly RepositorySelector _repositorySelector;

    public GameController(BoardView boardView, TicTacToeService gameService,
        ScoreService scoreService, RepositorySelector repositorySelector)
    {
        _boardView = boardView;
        _gameService = gameService;
        _scoreService = scoreService;
        _repositorySelector = repositorySelector;
    }

    internal void StartGame(GameMode mode, TimerOption timer, UserSession session)
    {
        bool playAgain = true;

        while (playAgain)
        {
            string playerTwo = GetPlayerTwo(mode);

            Symbol playerOneSymbol;
            Symbol playerTwoSymbol;

            if (session.IsStart)
            {
                playerOneSymbol = Symbol.X;
                playerTwoSymbol = Symbol.O;
            }
            else
            {
                playerOneSymbol = Symbol.O;
                playerTwoSymbol = Symbol.X;
            }

            session.IsStart = !session.IsStart;

            GameRecord game = new GameRecord()
            {
                PlayerOneName = session.UserName,
                PlayerTwoName = playerTwo,
                PlayerOneSymbol = playerOneSymbol,
                PlayerTwoSymbol = playerTwoSymbol,
                Mode = mode,
                PlayedAt = DateTime.Now,
                TimeLimitSeconds = timer
            };

            Symbol[] board = Enumerable.Repeat(Symbol.Empty, 9).ToArray();

            _boardView.RunMatch(
                board,
                game.PlayerOneName,
                game.PlayerTwoName,
                playerOneSymbol,
                playerTwoSymbol,
                mode,
                timer,
                () => PlayGame(board, game, mode, timer));

            if (game.Result == GameResult.None)
            {
                Ui.Info("Game discarded.\n");
                return;
            }

            if (game.EndReason != EndReason.Timeout)
            {
                game.EndReason = EndReason.Normal;
            }

            // Save completed game
            _repositorySelector.GetGameRepository(session).SaveGame(game);

            // Update player score
            if (game.Result == GameResult.Win)
            {
                _scoreService.RecordWin(session.UserName, mode, session);
            }
            else if (game.Result == GameResult.Loss)
            {
                _scoreService.RecordLoss(session.UserName, session);
            }
            else
            {
                _scoreService.RecordDraw(session.UserName, session);
            }

            Ui.Info("Game saved successfully.\n");

            // Ask again only in manual mode
            if (mode == GameMode.Manual)
            {
                playAgain = _boardView.AskPlayAgain();
            }
            else
            {
                playAgain = false;
            }
        }
    }

    private void PlayGame(Symbol[] board, GameRecord game,
        GameMode mode, TimerOption timer)
    {
        Symbol currentTurn = Symbol.X;
        bool completed = false;
        bool quitGame = false;
        string resultMessage = string.Empty;

        while (!completed && !quitGame)
        {
            bool isPlayerOneTurn = currentTurn == game.PlayerOneSymbol;

            string currentPlayer;

            if (isPlayerOneTurn)
            {
                currentPlayer = game.PlayerOneName;
            }
            else
            {
                currentPlayer = game.PlayerTwoName;
            }

            bool isComputerTurn = mode != GameMode.Manual && !isPlayerOneTurn;

            int move;
            int remainingSeconds;

            if (isComputerTurn)
            {
                move = GetComputerMove(board, currentTurn, mode);

                Thread.Sleep(400);
                remainingSeconds = (int)timer;
            }
            else
            {
                move = _boardView.GetPlayerMove(currentTurn);
                remainingSeconds = _boardView.LastMoveRemainingSeconds;
            }

            // Check whether player wants to quit
            if (move == BoardView.QuitMove)
            {
                quitGame = true;
                break;
            }

            // Check whether player ran out of time
            if (move == BoardView.TimeoutMove)
            {
                game.EndReason = EndReason.Timeout;

                if (isPlayerOneTurn)
                {
                    game.WinnerName = game.PlayerTwoName;
                    game.Result = GameResult.Loss;
                }
                else
                {
                    game.WinnerName = game.PlayerOneName;
                    game.Result = GameResult.Win;
                }

                resultMessage = $"[bold red]Time's up! {game.WinnerName} wins![/]";

                completed = true;
                break;
            }

            // Update the board
            board[move] = currentTurn;

            // Store the move
            game.Moves.Add(new MoveRecord()
            {
                Cell = move,
                Symbol = currentTurn,
                Timestamp = DateTime.Now,
                RemainingSeconds = remainingSeconds
            });

            _boardView.UpdateBoard(_gameService.ToggleSymbol(currentTurn));

            // Check for winner
            Symbol winner = _gameService.IsWinCondition(board);

            if (winner != Symbol.Empty)
            {
                if (winner == game.PlayerOneSymbol)
                {
                    game.WinnerName = game.PlayerOneName;
                    game.Result = GameResult.Win;
                }
                else
                {
                    game.WinnerName = game.PlayerTwoName;
                    game.Result = GameResult.Loss;
                }

                resultMessage = $"[bold green]{game.WinnerName} ({winner}) won the match![/]";

                completed = true;
                break;
            }

            // Check for draw
            if (_gameService.IsDraw(board))
            {
                game.Result = GameResult.Draw;
                game.WinnerName = string.Empty;

                resultMessage = "[bold yellow]It's a draw![/]";

                completed = true;
                break;
            }

            // Change turn
            currentTurn = _gameService.ToggleSymbol(currentTurn);
        }

        if (completed)
        {
            _boardView.DisplayResult(resultMessage);
        }
    }

    private string GetPlayerTwo(GameMode mode)
    {
        if (mode == GameMode.Manual)
        {
            return _boardView.GetPlayerName();
        }

        return "Computer";
    }

    private int GetComputerMove(Symbol[] board, Symbol symbol, GameMode mode)
    {
        if (mode == GameMode.Easy)
        {
            return _easyStrategy.GetNextMove(board, symbol);
        }
        else if (mode == GameMode.Medium)
        {
            return _mediumStrategy.GetNextMove(board, symbol);
        }
        else if (mode == GameMode.Hard)
        {
            return _hardStrategy.GetNextMove(board, symbol);
        }

        return -1;
    }

    internal void ShowHistory(UserSession session)
    {
        List<GameRecord> games =
            _repositorySelector.GetGameRepository(session).GetAll();

        GameRecord? selectedGame = _boardView.SelectGame(games);

        if (selectedGame != null)
        {
            _boardView.ReplayGame(selectedGame);
        }
    }
}
