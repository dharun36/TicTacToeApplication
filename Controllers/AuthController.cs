using TicTacToeApplication.Models;
using TicTacToeApplication.Models.Enums;
using TicTacToeApplication.Services;
using TicTacToeApplication.Views;

namespace TicTacToeApplication.Controllers;

/// <summary>
/// Controls the authentication flow in the application.
/// </summary>
internal class AuthController
{
    private readonly AuthService _authService;
    private readonly AuthView _authView;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">Instance of the authentication service.</param>
    /// <param name="authView">Instance of the authentication view.</param>
    public AuthController(AuthService authService, AuthView authView)
    {
        _authService = authService;
        _authView = authView;
    }

    /// <summary>
    /// Performs authentication.
    /// </summary>
    /// <returns>The user session created.</returns>
    public UserSession? HandleAuth()
    {
        while (true)
        {
            try
            {
                WelcomeChoice choice = _authView.ShowWelcomeMenu();
                switch (choice)
                {
                    case WelcomeChoice.Login:
                        UserSession? session = HandleLogIn();

                        if (session != null)
                        {
                            return session;
                        }

                        _authView.DisplayError("Please enter a valid username and password");
                        break;

                    case WelcomeChoice.SignUp:
                        HandleSignUp();
                        break;

                    case WelcomeChoice.Guest:
                        return _authService.LogInGuest();

                    case WelcomeChoice.Exit:
                        return null;
                }

                _authView.Pause();
            }
            catch (Exception e)
            {
                _authView.DisplayError(e.Message);
            }
        }
    }

    private UserSession? HandleLogIn()
    {
        _authView.DisplayTitle("LOGIN PAGE");

        string username = _authView.GetUserName();
        string password = _authView.GetPassword();

        return _authService.LogIn(username, password);
    }

    private void HandleSignUp()
    {
        _authView.DisplayTitle("SIGN UP PAGE");

        string username = _authView.GetUserName();
        string name = _authView.GetName();
        string email = _authView.GetEmail();
        string password = _authView.GetPassword();

        var (success, message) = _authService.CreateUser(username, name, password, email);

        if (success)
        {
            _authView.DisplaySuccess(message);
        }
        else
        {
            _authView.DisplayError(message);
        }
    }
}
