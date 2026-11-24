
using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Rendering;
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Views;

internal class BoardView
{
    public const int QuitMove = -1;
    public const int TimeoutMove = -2;

    /// <summary>
    /// Starts one persistent Live rendering session.
    /// </summary>
    internal void RunLive(
        Symbol[] board,
        string playerName,
        GameMode mode,
        TimerOption timer,
        Action<Action<Renderable>> gameLoop)
    {
        Renderable initialDisplay = RenderGame(
            board, -1, Symbol.X, playerName,
            mode, timer, (int)timer);

        AnsiConsole.Live(initialDisplay)
            .AutoClear(false)
            .Start(context =>
            {
                Action<Renderable> update =
                    renderable => context.UpdateTarget(renderable);

                gameLoop(update);
            });
    }

    /// <summary>
    /// Reads human input and displays the countdown.
    /// </summary>
    internal int GetPlayerMove(
        Symbol[] board,
        Symbol currentSymbol,
        string playerName,
        GameMode mode,
        TimerOption timer,
        Action<Renderable> update)
    {
        int selectedIndex = Array.FindIndex(
            board, cell => cell == Symbol.Empty);

        if (selectedIndex == -1)
            return QuitMove;

        int timeLimit = (int)timer;

        // Start a new timer for this human move.
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            int remainingSeconds = timeLimit == 0
                ? 0
                : Math.Max(
                    0,
                    (int)Math.Ceiling(
                        timeLimit - stopwatch.Elapsed.TotalSeconds));

            // Update the existing screen with the countdown.
            update(RenderGame(
                board,
                selectedIndex,
                currentSymbol,
                playerName,
                mode,
                timer,
                remainingSeconds));

            // Check timeout.
            if (timeLimit > 0 &&
                stopwatch.Elapsed.TotalSeconds >= timeLimit)
            {
                return TimeoutMove;
            }

            if (!Console.KeyAvailable)
            {
                Thread.Sleep(50);
                continue;
            }

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex =
                        (selectedIndex - 3 + 9) % 9;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex =
                        (selectedIndex + 3) % 9;
                    break;

                case ConsoleKey.LeftArrow:
                    selectedIndex =
                        (selectedIndex - 1 + 9) % 9;
                    break;

                case ConsoleKey.RightArrow:
                    selectedIndex =
                        (selectedIndex + 1) % 9;
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:

                    if (board[selectedIndex] == Symbol.Empty)
                        return selectedIndex;

                    break;

                case ConsoleKey.Q:
                    return QuitMove;
            }
        }
    }

    /// <summary>
    /// Updates the board without creating another Live session.
    /// </summary>
    internal void UpdateBoard(
        Action<Renderable> update,
        Symbol[] board,
        Symbol currentSymbol,
        string playerName,
        GameMode mode,
        TimerOption timer)
    {
        update(RenderGame(
            board,
            -1,
            currentSymbol,
            playerName,
            mode,
            timer,
            (int)timer));
    }

    /// <summary>
    /// Displays the final result in the existing Live region.
    /// </summary>
    internal void DisplayResult(
        Action<Renderable> update,
        Symbol[] board,
        Symbol winner,
        string playerName,
        GameMode mode,
        TimerOption timer)
    {
        string result = winner == Symbol.Empty
            ? "[bold yellow]MATCH DRAW![/]"
            : $"[bold green]PLAYER {winner} WINS![/]";

        update(new Rows(
            RenderGame(
                board,
                -1,
                winner,
                playerName,
                mode,
                timer,
                (int)timer),

            Align.Center(new Markup($"\n{result}")),

            Align.Center(
                new Markup("\n[grey]Game completed[/]"))
        ));
    }

    /// <summary>
    /// Renders the complete game screen.
    /// </summary>
    private Renderable RenderGame(
        Symbol[] board,
        int selectedIndex,
        Symbol currentSymbol,
        string playerName,
        GameMode mode,
        TimerOption timer,
        int remainingSeconds)
    {
        // Top timer: configured time limit.
        string timerText = timer == TimerOption.None
            ? "Untimed"
            : $"Timed, {(int)timer} s per move";

        // Bottom timer: remaining time for human turn.
        string countdown = timer == TimerOption.None
            ? string.Empty
            : $" · [bold yellow]{remainingSeconds} s[/]";

        string turnText = currentSymbol == Symbol.X
            ? $"[bold green]Your turn ({currentSymbol})[/]{countdown}"
            : "[bold green]Computer's turn (O)[/]";

        string player = Markup.Escape(playerName);

        Table gameBoard = new Table()
            .Border(TableBorder.Heavy)
            .BorderColor(Color.Aqua)
            .HideHeaders()
            .ShowRowSeparators();

        for (int i = 0; i < 3; i++)
        {
            gameBoard.AddColumn(
                new TableColumn(" ")
                    .Centered()
                    .Padding(5, 2));
        }

        for (int i = 0; i < 9; i += 3)
        {
            gameBoard.AddRow(
                FormatCell(board[i], i == selectedIndex),
                FormatCell(board[i + 1], i + 1 == selectedIndex),
                FormatCell(board[i + 2], i + 2 == selectedIndex));
        }

        return new Rows(
            Align.Center(
                new Markup("[bold aqua]TIC TAC TOE[/]")),

            Align.Center(
                new Markup(
                    $"[grey]{Markup.Escape(mode.ToString())} · {Markup.Escape(timerText)}[/]")),

            Align.Center(
                new Markup(
                    $"\n[bold aqua]{player} (X)[/]    [grey]vs[/]    [bold orange1]Computer (O)[/]\n")),

            Align.Center(gameBoard),

            Align.Center(
                new Markup($"\n{turnText}")),

            Align.Center(
                new Markup("\n[grey]Q  quit[/]"))
        );
    }

    /// <summary>
    /// Formats an individual board cell.
    /// </summary>
    private static string FormatCell(
        Symbol symbol,
        bool selected)
    {
        string value = symbol == Symbol.Empty
            ? " "
            : symbol.ToString();

        string cell = $"  {value}  ";

        if (selected)
            return $"[black on grey]{cell}[/]";

        return symbol switch
        {
            Symbol.X => $"[bold aqua]{cell}[/]",
            Symbol.O => $"[bold orange1]{cell}[/]",
            _ => cell
        };
    }
}
