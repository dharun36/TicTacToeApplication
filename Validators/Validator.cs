using System.Text.RegularExpressions;

namespace TicTacToeApplication.Validators;

/// <summary>
/// Contains the validators.
/// </summary>
internal static class Validator
{
    /// <summary>
    /// Minimum length of the password.
    /// </summary>
    internal const int MinimumPasswordLength = 8;

    /// <summary>
    /// Regex pattern for email validation.
    /// </summary>
    internal const string Pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    /// <summary>
    /// Minimum length of the name.
    /// </summary>
    internal const int MinimumNameLength = 5;

    /// <summary>
    /// Validates email.
    /// </summary>
    /// <param name="email"> Email of the contact. </param>
    /// <returns>True if email is valid; otherwise false. </returns>
    internal static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return Regex.IsMatch(email, Pattern);
    }

    /// <summary>
    /// Validates optional parameter notes.
    /// </summary>
    /// <param name="names">Notes. </param>
    /// <returns>True if the notes is valid; otherwise false.</returns>
    internal static bool IsValidName(string names)
    {
        if (string.IsNullOrWhiteSpace(names) && names.Length < MinimumNameLength)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates the password entered by the user.
    /// </summary>
    /// <param name="password">The password entered by the user.</param>
    /// <returns>True if the password is valid; otherwise false.</returns>
    internal static bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length >= MinimumPasswordLength;
    }
}
