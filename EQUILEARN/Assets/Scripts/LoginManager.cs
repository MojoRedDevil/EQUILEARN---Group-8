using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;

public class LoginManager : MonoBehaviour
{
    // Input fields for the user's email and password
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    // Buttons for submitting login information, resetting password, and creating a new account
    public Button loginButton;
    public Button forgotPasswordButton;
    public Button createAccountButton;

    // Text field for displaying login feedback messages
    public TextMeshProUGUI feedbackText;

    private FirebaseAuth auth; // Firebase Authentication reference
    private Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"); // Regex to validate email format

    [SerializeField] private string homepageSceneName = "HomePage"; // Scene to load upon successful login

    void Start()
    {
        // Initialize Firebase Auth and set button click listeners
        auth = FirebaseAuth.DefaultInstance;

        loginButton.onClick.AddListener(OnLoginButtonClicked);
        forgotPasswordButton.onClick.AddListener(OnForgotPasswordClicked);
        createAccountButton.onClick.AddListener(OnCreateAccountClicked);

        // Set password input field to obscure text for security
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
    }

    private void Login(string email, string password)
    {
        // Attempt to sign in with email and password
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            // Handle task result, including errors and successful login
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                UpdateFeedback("Login was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                UpdateFeedback("Error during login.");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            // Reload user data to ensure it is current
            newUser.ReloadAsync().ContinueWithOnMainThread(reloadTask =>
            {
                // Handle reload task result, including email verification status
                if (reloadTask.IsFaulted || reloadTask.IsCanceled)
                {
                    UpdateFeedback("Failed to reload user data.");
                    return;
                }

                if (!newUser.IsEmailVerified)
                {
                    SceneManager.LoadScene("EmailVerification");
                }
                else
                {
                    SceneManager.LoadScene(homepageSceneName);
                }
            });
        });
    }

    private void OnLoginButtonClicked()
    {
        // Validate email and password format before attempting to login
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (IsEmailValid(email) && IsPasswordValid(password))
        {
            Login(email, password);
        }
        else
        {
            UpdateFeedback("Invalid email or password format.");
        }
    }

    private void OnForgotPasswordClicked()
    {
        // Navigate to the Forgot Password scene
        SceneManager.LoadScene("ForgotPassword");
    }

    private void OnCreateAccountClicked()
    {
        // Navigate to the Sign Up scene
        SceneManager.LoadScene("SignUp");
    }

    private bool IsEmailValid(string email)
    {
        // Check if email matches the regex pattern
        return emailRegex.IsMatch(email);
    }

    private bool IsPasswordValid(string password)
    {
        // Check if password meets the minimum length requirement
        return password.Length >= 8;
    }

    private void UpdateFeedback(string message)
    {
        // Display feedback message to the user
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
    }
}
