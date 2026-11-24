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
    internal MainMenuOption GetMenuOption(UserSession session)
    {
        AnsiConsole.Clear();

        Banner("MAIN MENU", session);

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
    internal void Banner(string title, UserSession session)
    {
        var playerInfo = new Panel(Align.Center(
            new Markup(
                $"[grey]Hello[/] [bold green]{Markup.Escape(session.UserName)}[/]\n" +
                $"[grey]Total point[/] [bold yellow]30 [/]" +
                $"[grey]Rank[/] [bold yellow] #5 [/]" +
                $"[bold green] 14 wins [/]")))
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
}
