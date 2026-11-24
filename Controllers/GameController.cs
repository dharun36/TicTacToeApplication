
using Spectre.Console.Rendering;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Services;
using TicTacToeApplication.Services.Strategies;
using TicTacToeApplication.Views;

namespace TicTacToeApplication.Controllers;

internal class GameController
{
    private readonly BoardView _boardView;
    private readonly TicTacToeService _gameService;

    private readonly EasyStrategy _easyStrategy;
    private readonly MediumStrategy _mediumStrategy;
    private readonly MinimaxAlgorithm _minimaxAlgorithm;

    public GameController(
        BoardView boardView,
        TicTacToeService gameService)
    {
        _boardView = boardView;
        _gameService = gameService;

        _easyStrategy = new EasyStrategy();
        _mediumStrategy = new MediumStrategy(gameService);
        _minimaxAlgorithm = new MinimaxAlgorithm(gameService);
    }

    internal void StartGame(
        GameMode mode,
        TimerOption timer,
        string playerName = "Player")
    {
        if (mode == GameMode.Manual)
        {
            Ui.Error("Manual mode is not implemented yet.");
            return;
        }

        Symbol[] board = Enumerable
            .Repeat(Symbol.Empty, 9)
            .ToArray();

        Symbol humanSymbol = Symbol.X;
        Symbol computerSymbol = Symbol.O;
        Symbol currentTurn = humanSymbol;

        bool gameFinished = false;
        bool quitGame = false;

        // Start exactly one Live session for the entire game.
        _boardView.RunLive(
            board,
            playerName,
            mode,
            timer,
            update =>
            {
                while (!gameFinished)
                {
                    // HUMAN TURN
                    if (currentTurn == humanSymbol)
                    {
                        int move = _boardView.GetPlayerMove(
                            board,
                            currentTurn,
                            playerName,
                            mode,
                            timer,
                            update);

                        // Handle quit.
                        if (move == BoardView.QuitMove)
                        {
                            quitGame = true;
                            gameFinished = true;
                            break;
                        }

                        // Handle timeout.
                        if (move == BoardView.TimeoutMove)
                        {
                            _boardView.DisplayResult(
                                update,
                                board,
                                computerSymbol,
                                playerName,
                                mode,
                                timer);

                            gameFinished = true;
                            break;
                        }

                        // Validate the selected cell.
                        if (move < 0 ||
                            move >= board.Length ||
                            board[move] != Symbol.Empty)
                        {
                            continue;
                        }

                        // Controller modifies the board.
                        board[move] = humanSymbol;
                    }
                    else
                    {
                        // COMPUTER TURN
                        int computerMove = GetComputerMove(
                            board,
                            computerSymbol,
                            mode);

                        board[computerMove] = computerSymbol;
                    }

                    // Update the existing Live display.
                    _boardView.UpdateBoard(
                        update,
                        board,
                        currentTurn,
                        playerName,
                        mode,
                        timer);

                    // Check winner.
                    Symbol winner =
                        _gameService.IsWinCondition(board);

                    if (winner != Symbol.Empty)
                    {
                        _boardView.DisplayResult(
                            update,
                            board,
                            winner,
                            playerName,
                            mode,
                            timer);

                        gameFinished = true;
                        break;
                    }

                    // Check draw.
                    if (_gameService.IsDraw(board))
                    {
                        _boardView.DisplayResult(
                            update,
                            board,
                            Symbol.Empty,
                            playerName,
                            mode,
                            timer);

                        gameFinished = true;
                        break;
                    }

                    // Switch turn.
                    currentTurn =
                        _gameService.ToggleSymbol(currentTurn);
                }
            });

        // Live session has ended here.
        if (quitGame)
        {
            Ui.Info("Game discarded.");
        }
    }

    private int GetComputerMove(
        Symbol[] board,
        Symbol computerSymbol,
        GameMode mode)
    {
        return mode switch
        {
            GameMode.Easy =>
                _easyStrategy.GetNextMove(
                    board,
                    computerSymbol),

            GameMode.Medium =>
                _mediumStrategy.GetNextMove(
                    board,
                    computerSymbol),

            GameMode.Hard =>
                _minimaxAlgorithm.GetBestMove(
                    board,
                    computerSymbol),

            _ => throw new ArgumentOutOfRangeException(
                nameof(mode),
                mode,
                "Unsupported game mode.")
        };
    }
}
