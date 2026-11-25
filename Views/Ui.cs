using Spectre.Console;

namespace TicTacToeApplication.Views;

/// <summary>
/// Provides helper methods for displaying messages, titles, and banners in the console.
/// </summary>
internal class Ui
{
    /// <summary>
    /// Displays message in grey text.
    /// </summary>
    /// <param name="message">The text message to display.</param>
    public static void Info(string message)
    {
        AnsiConsole.Markup("[grey]" + Markup.Escape(message) + "[/]");
    }

    /// <summary>
    /// Displays message in green text.
    /// </summary>
    /// <param name="message">The success message to display.</param>
    public static void Success(string message)
    {
        AnsiConsole.Markup("[green]" + Markup.Escape(message) + "[/]");
    }

    /// <summary>
    /// Displays message in red text.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    public static void Error(string message)
    {
        AnsiConsole.Markup("[red]" + Markup.Escape(message) + "[/]");
    }

    /// <summary>
    /// Pauses the console and waits for the user to press any key.
    /// </summary>
    public static void Pause()
    {
        AnsiConsole.Markup("[grey]Press any key to continue...[/]");
        Console.ReadKey();
    }

    /// <summary>
    /// Asks the user a yes-or-no confirmation question.
    /// </summary>
    /// <param name="message">The confirmation question to display.</param>
    /// <returns>True if the user confirms; otherwise, false.</returns>
    public static bool Confirm(string message)
    {
        return AnsiConsole.Confirm(message);
    }

    /// <summary>
    /// Displays yellow title with center alignment.
    /// </summary>
    /// <param name="title">The title text to print.</param>
    public static void Title(string title)
    {
        AnsiConsole.Write(new Rule("[bold yellow]" + title + "[/]").RuleStyle("grey"));
    }

    /// <summary>
    /// Displays a bordered banner box with a main title and subtitle text.
    /// </summary>
    /// <param name="title">The main heading inside the banner.</param>
    /// <param name="markupBody">The description text shown below the heading.</param>
    public static void Banner(string title, string markupBody)
    {
        var panel = new Panel(Align.Center(
            new Markup(
                $"[bold aqua]{Markup.Escape(title)}[/]\n" +
                $"{markupBody}")))
            .Border(BoxBorder.Rounded)
            .BorderStyle(Color.Aqua)
            .Expand();

        AnsiConsole.Write(panel);
    }
}
