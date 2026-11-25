using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Rendering;
using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Validators;

namespace TicTacToeApplication.Views;

internal class BoardView
{
    public const int QuitMove = -1;
    public const int TimeoutMove = -2;

    private Symbol[] _board = Array.Empty<Symbol>();
    private string _playerOne = string.Empty;
    private string _playerTwo = string.Empty;
    private Symbol _playerOneSymbol;
    private Symbol _playerTwoSymbol;
    private GameMode _mode;
    private TimerOption _timer;
    private LiveDisplayContext? _liveContext;

    internal int LastMoveRemainingSeconds { get; private set; }

    internal void RunMatch(Symbol[] board, string playerOne, string playerTwo,
        Symbol playerOneSymbol, Symbol playerTwoSymbol, GameMode mode,
        TimerOption timer, Action gameLoop)
    {
        _board = board;
        _playerOne = playerOne;
        _playerTwo = playerTwo;
        _playerOneSymbol = playerOneSymbol;
        _playerTwoSymbol = playerTwoSymbol;
        _mode = mode;
        _timer = timer;

        AnsiConsole.Clear();
        AnsiConsole.Live(BuildScreen(-1, Symbol.X, (int)timer))
            .AutoClear(false)
            .Start(ctx =>
            {
                _liveContext = ctx;
                gameLoop();
                _liveContext = null;
            });
    }

    internal int GetPlayerMove(Symbol currentSymbol)
    {
        int selectedIndex = Array.FindIndex(_board, cell => cell == Symbol.Empty);
        if (selectedIndex == -1) return QuitMove;

        int timeLimit = (int)_timer;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            int remaining = timeLimit == 0 ? 0 :
                Math.Max(0, (int)Math.Ceiling(timeLimit - stopwatch.Elapsed.TotalSeconds));

            LastMoveRemainingSeconds = remaining;
            _liveContext?.UpdateTarget(BuildScreen(selectedIndex, currentSymbol, remaining));

            if (timeLimit > 0 && stopwatch.Elapsed.TotalSeconds >= timeLimit)
                return TimeoutMove;

            if (!Console.KeyAvailable)
            {
                Thread.Sleep(50);
                continue;
            }

            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow: selectedIndex = (selectedIndex - 3 + 9) % 9; break;
                case ConsoleKey.DownArrow: selectedIndex = (selectedIndex + 3) % 9; break;
                case ConsoleKey.LeftArrow: selectedIndex = (selectedIndex - 1 + 9) % 9; break;
                case ConsoleKey.RightArrow: selectedIndex = (selectedIndex + 1) % 9; break;
                case ConsoleKey.Escape:
                case ConsoleKey.Q: return QuitMove;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (_board[selectedIndex] == Symbol.Empty) return selectedIndex;
                    break;
            }
        }
    }

    internal void UpdateBoard(Symbol currentSymbol)
    {
        _liveContext?.UpdateTarget(BuildScreen(-1, currentSymbol, (int)_timer));
    }

    internal void DisplayResult(string message)
    {
        _liveContext?.UpdateTarget(BuildScreen(-1, Symbol.Empty, 0, message));
    }

    private IRenderable BuildScreen(int selectedIndex, Symbol currentTurn, int remainingSeconds, string? result = null)
    {
        string turnName = currentTurn == _playerOneSymbol ? _playerOne : _playerTwo;
        var header = new Markup(
            $"[bold aqua]TIC TAC TOE[/]\n" +
            $"[green]{Markup.Escape(_playerOne)} ({_playerOneSymbol})[/]  Vs  " +
            $"[yellow]{Markup.Escape(_playerTwo)} ({_playerTwoSymbol})[/]\n" +
            $"[grey]Mode: {_mode}[/]");

        string statusLine = result ?? (
            _timer == TimerOption.None
                ? $"Turn: [bold]{Markup.Escape(turnName)} ({currentTurn})[/]  [grey](Arrows, Enter, Esc to quit)[/]"
                : $"Turn: [bold]{Markup.Escape(turnName)} ({currentTurn})[/]  |  Time remaining: [bold yellow]{remainingSeconds}s[/]");

        Table table = new Table().Border(TableBorder.Heavy).BorderColor(Color.Aqua)
            .HideHeaders().ShowRowSeparators();
        for (int i = 0; i < 3; i++) table.AddColumn(new TableColumn(" ").Centered().Padding(1, 0));

        for (int i = 0; i < 9; i += 3)
            table.AddRow(FormatCell(_board[i], i == selectedIndex),
                FormatCell(_board[i + 1], i + 1 == selectedIndex),
                FormatCell(_board[i + 2], i + 2 == selectedIndex));

        return Align.Center(new Rows(Align.Center(header), new Text(""),
            Align.Center(table), new Text(""), Align.Center(new Markup(statusLine))));
    }

    private string FormatCell(Symbol symbol, bool selected)
    {
        string padded = $"  {(symbol == Symbol.Empty ? " " : symbol.ToString())}  ";
        if (selected) return $"[black on aqua]{padded}[/]";
        return symbol switch
        {
            Symbol.X => $"[bold aqua]{padded}[/]",
            Symbol.O => $"[bold yellow]{padded}[/]",
            _ => padded
        };
    }

    internal string GetPlayerName()
    {
        int attempts = 3;
        string name = AnsiConsole.Prompt(new TextPrompt<string>("Enter the username of player 2: "));
        while (!Validator.IsValidName(name))
        {
            if (attempts == 1) throw new InvalidDataException("No attempt left. Please try again.");
            Ui.Error($"Enter a valid username with minimum {Validator.MinimumNameLength} characters\n");
            Ui.Error($"Tries left: {--attempts}\n");
            name = AnsiConsole.Prompt(new TextPrompt<string>("Enter the username of player 2: "));
        }
        return name;
    }

    internal GameRecord? SelectGame(List<GameRecord> games)
    {
        AnsiConsole.Clear();
        Ui.Title("GAME HISTORY");
        if (games.Count == 0)
        {
            Ui.Info("No completed games found.\n");
            return null;
        }

        var choices = games.OrderByDescending(g => g.PlayedAt)
            .Select(g => g).ToList();
        var selected = AnsiConsole.Prompt(new SelectionPrompt<GameRecord>()
            .Title("Select a game to replay")
            .UseConverter(g => $"{g.PlayedAt:g} | {g.PlayerOneName} vs {g.PlayerTwoName} | {g.Result}")
            .AddChoices(choices));
        return selected;
    }

    internal bool AskPlayAgain()
    {
        AnsiConsole.WriteLine();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What do you want to do?")
                .AddChoices("Play Again", "Exit"));

        if (choice == "Play Again")
        {
            return true;
        }

        return false;
    }

    internal void ReplayGame(GameRecord game)
    {
        Symbol[] board = Enumerable.Repeat(Symbol.Empty, 9).ToArray();

        AnsiConsole.Clear();

        Ui.Title($"REPLAY: {game.PlayerOneName} vs {game.PlayerTwoName}");

        foreach (MoveRecord move in game.Moves)
        {
            board[move.Cell] = move.Symbol;

            string playerName = move.Symbol == game.PlayerOneSymbol
                ? game.PlayerOneName
                : game.PlayerTwoName;

            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                $"[bold aqua]{Markup.Escape(playerName)} ({move.Symbol})[/] moved to cell {move.Cell + 1}");

            AnsiConsole.MarkupLine(
                $"[grey]Time remaining: {move.RemainingSeconds}s | {move.Timestamp:T}[/]");

            Table table = new Table()
                .Border(TableBorder.Heavy)
                .HideHeaders();

            for (int i = 0; i < 3; i++)
            {
                table.AddColumn(" ");
            }

            for (int i = 0; i < 9; i += 3)
            {
                table.AddRow(
                    ReplayCell(board[i]),
                    ReplayCell(board[i + 1]),
                    ReplayCell(board[i + 2]));
            }

            AnsiConsole.Write(Align.Center(table));

            AnsiConsole.MarkupLine(
                "[grey]Press any key for next move...[/]");

            Console.ReadKey(true);
        }

        AnsiConsole.MarkupLine($"\n[bold yellow]Result: {game.Result}[/]");

        AnsiConsole.MarkupLine(
            $"[grey]Winner: {Markup.Escape(game.WinnerName)} | End: {game.EndReason}[/]");
    }

    private string ReplayCell(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.X => "[bold aqua] X [/]",
            Symbol.O => "[bold yellow] O [/]",
            _ => "   "
        };
    }
}
