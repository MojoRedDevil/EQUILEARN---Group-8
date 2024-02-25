using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class ResetPasswordManager : MonoBehaviour
{
    // UI elements for user interaction
    public Button resendEmailButton;
    public TextMeshProUGUI feedbackText; // Displays feedback messages to the user
    public TextMeshProUGUI verificationMessageText; // Displays instructions or verification messages to the user
    private FirebaseAuth auth; // Firebase Authentication reference
    private string userEmail; // Email address of the user
    public Button loginButton; // Button to navigate back to the login scene

    void Start()
    {
        // Initialize Firebase Authentication and retrieve the user's email from the ForgotPasswordManager
        auth = FirebaseAuth.DefaultInstance;
        userEmail = ForgotPasswordManager.UserEmail;

        // Setup listener for the login button to navigate back to the login scene
        loginButton.onClick.AddListener(OnLoginButtonClicked);

        // Display a message to the user about the password reset email
        if (!string.IsNullOrEmpty(userEmail))
        {
            verificationMessageText.text = $"An email has been sent to your email address {userEmail}. Please click on the link to reset your password.";
        }

        // Setup listener for the resend email button to resend the password reset email
        resendEmailButton.onClick.AddListener(OnResendEmailButtonClicked);
    }

    private void OnResendEmailButtonClicked()
    {
        // Resend the password reset email if we have the user's email
        if (!string.IsNullOrEmpty(userEmail))
        {
            SendPasswordResetEmail(userEmail);
        }
    }

    private void SendPasswordResetEmail(string email)
    {
        // Attempt to send a password reset email and update the feedback text based on the outcome
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                feedbackText.text = "Password reset was canceled.";
            }
            else if (task.IsFaulted)
            {
                feedbackText.text = "Error: " + task.Exception.Message;
            }
            else
            {
                feedbackText.text = "Password reset email sent again. Please check your inbox.";
            }
        });
    }

    private void OnLoginButtonClicked()
    {
        // Navigate back to the login scene when the login button is clicked
        SceneManager.LoadScene("Login");
    }
}
