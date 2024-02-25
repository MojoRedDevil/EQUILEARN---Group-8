using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ForgotPasswordManager : MonoBehaviour
{
    // UI elements for user input and interaction
    public TMP_InputField emailInputField;
    public Button resetButton;
    public TextMeshProUGUI feedbackText; // Displays feedback messages to the user
    public Button loginButton;

    private FirebaseAuth auth; // Firebase Authentication reference
    private Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"); // Regex to validate email format

    public static string UserEmail { get; private set; } // Static variable to store user's email

    void Start()
    {
        // Initialize Firebase Authentication and setup button click listeners
        auth = FirebaseAuth.DefaultInstance;
        resetButton.onClick.AddListener(OnResetButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void SendPasswordResetEmail(string email)
    {
        // Attempt to send a password reset email
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                UpdateFeedback("Password reset was canceled.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                UpdateFeedback("Error: " + task.Exception.Message);
            }
            else
            {
                // On successful email send, update user's email and navigate to the reset password screen
                UserEmail = email;
                SceneManager.LoadScene("ResetPassword");
            }
        });
    }

    private void OnResetButtonClicked()
    {
        // Validate email and attempt to send a password reset email
        string email = emailInputField.text;
        if (IsEmailValid(email))
        {
            SendPasswordResetEmail(email);
        }
        else
        {
            UpdateFeedback("Please enter a valid email address.");
        }
    }

    private void OnLoginButtonClicked()
    {
        // Navigate to the login scene
        SceneManager.LoadScene("Login");
    }

    private bool IsEmailValid(string email)
    {
        // Check if email matches the regex pattern for validation
        return emailRegex.IsMatch(email);
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
