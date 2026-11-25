using Spectre.Console;
using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;

namespace TicTacToeApplication.Views;

/// <summary>
/// Handles the main screens, menus, and banners shown to the user in the console.
/// </summary>
internal class AppView
{
    /// <summary>
    /// Displays the main menu and gets the option chosen by the user.
    /// </summary>
    /// <param name="session">The current logged-in user session.</param>
    /// <returns>The selected <see cref="MainMenuOption"/>.</returns>
    internal MainMenuOption GetMenuOption(UserSession session, int totalPoints, int wins)
    {
        AnsiConsole.Clear();

        Banner("MAIN MENU", session.UserName, totalPoints, wins);

        var mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option to continue")
                    .AddChoices("Play New Game", "History", "Profile", "Leaderboard", "Logout"));

        return mainMenu switch
        {
            "Play New Game" => MainMenuOption.NewGame,
            "History" => MainMenuOption.History,
            "Profile" => MainMenuOption.Profile,
            "Leaderboard" => MainMenuOption.LeaderBoard,
            "Logout" => MainMenuOption.Logout,
            _ => throw new InvalidOperationException("Invalid choice")
        };
    }

    /// <summary>
    /// Displays the game mode menu and gets the difficulty or mode selected by the user.
    /// </summary>
    /// <returns>The selected <see cref="GameMode"/>.</returns>
    internal GameMode GetGameMode()
    {
        AnsiConsole.Clear();

        Ui.Title("Game Modes");

        var mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a game mode to start")
                    .AddChoices("Easy", "Medium", "Hard", "Manual"));

        return mainMenu switch
        {
            "Easy" => GameMode.Easy,
            "Medium" => GameMode.Medium,
            "Hard" => GameMode.Hard,
            "Manual" => GameMode.Manual,
            _ => throw new InvalidOperationException("Invalid choice")
        };
    }

    /// <summary>
    /// Displays a banner at the top of the screen with the title and player details.
    /// </summary>
    /// <param name="title">The heading text to show on the banner.</param>
    /// <param name="session">The current user session containing player details.</param>
    internal void Banner(string title, string userName, int totalPoints, int wins)
    {
        var playerInfo = new Panel(Align.Center(
            new Markup(
                $"[grey]Hello[/] [bold green]{Markup.Escape(userName)}[/]\n" +
                $"[grey]Total point[/] [bold yellow]{totalPoints} [/]" +
                $"[bold green] {wins} wins [/]")))
            .Header("[bold aqua]" + title + "[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .BorderStyle(Color.Aqua)
            .Expand();

        AnsiConsole.Write(playerInfo);
    }

    /// <summary>
    /// Displays the timer options menu and gets the time limit chosen by the user.
    /// </summary>
    /// <returns>The selected <see cref="TimerOption"/>.</returns>
    internal TimerOption GetTimerOption()
    {
        AnsiConsole.Clear();

        Ui.Title("Timer Options");

        var mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a timer option")
                    .AddChoices("No time limit", "Five second per move", "Ten second per move"));

        return mainMenu switch
        {
            "No time limit" => TimerOption.None,
            "Five second per move" => TimerOption.FiveSecond,
            "Ten second per move" => TimerOption.TenSecond,
            _ => throw new InvalidOperationException("Invalid choice")
        };
    }

    internal void DisplayLeaderBoard(string userName, List<(string Username, int TotalPoints, int Wins)> leaderboard)
    {
        AnsiConsole.Clear();
        Ui.Title("LEADERBOARD (TOP 20)");

        if (leaderboard.Count == 0)
        {
            Ui.Info("No scores recorded yet.\n");
            Ui.Pause();
            return;
        }

        Table table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Aqua);

        table.AddColumn(new TableColumn("[bold yellow]Rank[/]").Centered());
        table.AddColumn(new TableColumn("[bold yellow]Username[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold yellow]Total Points[/]").Centered());
        table.AddColumn(new TableColumn("[bold yellow]Wins[/]").Centered());

        for (int i = 0; i < leaderboard.Count; i++)
        {
            var entry = leaderboard[i];
            int rank = i + 1;
            bool isCurrentUser = entry.Username.Equals(userName, StringComparison.OrdinalIgnoreCase);

            if (isCurrentUser)
            {
                table.AddRow(
                    $"[aqua] #{rank} [/]",
                    $"[aqua] {Markup.Escape(entry.Username)} (You) [/]",
                    $"[aqua] {entry.TotalPoints} [/]",
                    $"[aqua] {entry.Wins} [/]");
            }
            else
            {
                string rankColor = rank switch
                {
                    1 => "bold gold",
                    2 => "bold silver",
                    3 => "bold yellow",
                    _ => "grey"
                };

                table.AddRow(
                    $"[{rankColor}]#{rank}[/]",
                    Markup.Escape(entry.Username),
                    $"[yellow]{entry.TotalPoints}[/]",
                    $"[green]{entry.Wins}[/]");
            }
        }

        AnsiConsole.Write(table);
    }

    internal void DisplayProfile(Score score)
    {
        AnsiConsole.Clear();

        Ui.Banner("PROFILE",
            $"Username: [grey]{score.Username}[/]\n");

        Ui.Banner("STATISTICS",
            $"Game played: [grey]{score.Wins + score.Losses + score.Draws}[/]\n" +
            $"Wins: [green]{score.Wins}[/]\n" +
            $"Losses: [red]{score.Losses}[/]\n" +
            $"Draws: [yellow]{score.Draws}[/]\n" +
            $"Total points: [yellow]{score.TotalPoints}[/]\n");

        var chart = new BarChart()
        .Width(50)
        .Label("[bold aqua]Points by Game Mode[/]")
        .CenterLabel()
        .AddItem("Easy", score.PointsByMode.GetValueOrDefault(GameMode.Easy), Color.Green)
        .AddItem("Medium", score.PointsByMode.GetValueOrDefault(GameMode.Medium), Color.Yellow)
        .AddItem("Hard", score.PointsByMode.GetValueOrDefault(GameMode.Hard), Color.Red)
        .AddItem("Manual", score.PointsByMode.GetValueOrDefault(GameMode.Manual), Color.Aqua);

        AnsiConsole.Write(chart);
    }

    internal bool ConfirmLogout()
    {
        return Ui.Confirm("Logging out will delete the user data\nDo you really want to quit?");
    }

    internal void DisplayInfo(string message)
    {
        Ui.Info(message);
    }
}