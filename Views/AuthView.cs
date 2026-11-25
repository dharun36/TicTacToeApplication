using Spectre.Console;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Validators;

namespace TicTacToeApplication.Views;

/// <summary>
/// Handles authentication screens shown to the user.
/// </summary>
internal class AuthView
{
    /// <summary>
    /// Displays an error message to the user.
    /// </summary>
    /// <param name="message">The error text to display.</param>
    internal void DisplayError(string message)
    {
        Ui.Error(message);
    }

    /// <summary>
    /// Displays a success message to the user.
    /// </summary>
    /// <param name="message">The success text to display.</param>
    internal void DisplaySuccess(string message)
    {
        Ui.Success(message);
    }

    /// <summary>
    /// Prompts the user to enter a valid email address.
    /// </summary>
    /// <returns>The validated email string.</returns>
    internal string GetEmail()
    {
        return GetValidatedInput(
            "Enter your email: ",
            false,
            Validator.IsValidEmail,
            "Enter a valid email in format (name@example.com)");
    }

    /// <summary>
    /// Prompts the user to enter their full name.
    /// </summary>
    /// <returns>The validated full name string.</returns>
    internal string GetName()
    {
        return GetValidatedInput(
            "Enter your full name: ",
            false,
            Validator.IsValidName,
            $"Enter a name with minimum {Validator.MinimumNameLength} characters");
    }

    /// <summary>
    /// Prompts the user to enter a valid password while hiding the characters.
    /// </summary>
    /// <returns>The validated password string.</returns>
    /// <exception cref="InvalidDataException">Thrown when the user runs out of valid password attempts.</exception>
    internal string GetPassword()
    {
        int remainingAttempts = 3;

        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[grey]Enter your password: [/]")
                .Secret('*'));

        while (!Validator.IsValidPassword(password))
        {
            if (remainingAttempts == 1)
            {
                throw new InvalidDataException("No attempt left, Please try again.\n");
            }

            Ui.Error($"Enter a valid password with minimum {Validator.MinimumPasswordLength} characters\n");
            Ui.Error($"Tries left: {--remainingAttempts}\n");

            password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your password: ")
                .PromptStyle("grey")
                .Secret('*'));
        }

        return password;
    }

    /// <summary>
    /// Prompts the user to enter a valid username.
    /// </summary>
    /// <returns>The validated username string.</returns>
    internal string GetUserName()
    {
        return GetValidatedInput(
            "Enter your username (unique): ",
            false,
            Validator.IsValidName,
            "Enter a valid username");
    }

    /// <summary>
    /// Displays the welcome screen and gets the starting option chosen by the user.
    /// </summary>
    /// <returns>The selected <see cref="WelcomeChoice"/>.</returns>
    internal WelcomeChoice ShowWelcomeMenu()
    {
        AnsiConsole.Clear();

        Ui.Banner("TIC TAC TOE", "Play score and climb the leaderboard");

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]What would you like to do[/]")
                .AddChoices("Login", "Sign up", "Play as guest", "Exit"));

        return choice switch
        {
            "Login" => WelcomeChoice.Login,
            "Sign up" => WelcomeChoice.SignUp,
            "Play as guest" => WelcomeChoice.Guest,
            "Exit" => WelcomeChoice.Exit,
            _ => throw new InvalidOperationException("Invalid choice")
        };
    }

    /// <summary>
    /// Pauses the screen until the user presses a key.
    /// </summary>
    internal void Pause()
    {
        Ui.Pause();
    }

    /// <summary>
    /// Displays a section title on the screen.
    /// </summary>
    /// <param name="title">The title text to display.</param>
    internal void DisplayTitle(string title)
    {
        Ui.Title(title);
    }

    private string GetString(string message)
    {
        Ui.Info(message);
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        return input;
    }

    private string GetValidatedInput(
        string prompt,
        bool isEditMode,
        Func<string, bool> isValidField,
        string errorMessage)
    {
        int remainingAttempts = 3;
        string input = GetString(prompt);
        if (isEditMode && string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        while (!isValidField(input))
        {
            if (remainingAttempts == 1)
            {
                throw new InvalidDataException("No attempt left, Please try again.\n");
            }

            Ui.Error(errorMessage);
            Ui.Error($"Tries left: {--remainingAttempts}\n");
            input = GetString(prompt);
        }

        return input;
    }
}
