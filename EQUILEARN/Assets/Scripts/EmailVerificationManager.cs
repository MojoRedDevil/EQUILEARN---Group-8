using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EmailVerificationManager : MonoBehaviour
{
    // UI element to display verification messages
    public TextMeshProUGUI verificationMessageText;
    // Buttons for resending verification email and navigating to login
    public Button resendEmailButton;
    public Button loginButton;

    private FirebaseAuth auth; // Firebase Authentication reference
    private FirebaseUser user; // Currently signed-in user

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        // Add click event listener for the login button
        loginButton.onClick.AddListener(OnLoginButtonClicked);

        if (user != null && !user.IsEmailVerified)
        {
            // Display a message indicating that a verification email has been sent
            verificationMessageText.text = $"Verification email sent to: {user.Email}\nPlease check your inbox and click the link to verify.";

            // Add click event listener for the resend email button to allow users to resend the verification email
            resendEmailButton.onClick.AddListener(OnResendEmailButtonClicked);
        }
        else
        {
            // If the user is already verified or not available, redirect to the login scene
            SceneManager.LoadScene("Login"); 
        }
    }

    private void OnResendEmailButtonClicked()
    {
        // Attempt to resend the verification email
        user.SendEmailVerificationAsync().ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                // Log an error if there is an issue resending the email
                Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
            }
            else
            {
                // Log or notify user of successful email resend
                Debug.Log("Verification email resent to: " + user.Email);               
            }
        });
    }

    private void OnLoginButtonClicked()
    {
        // Redirect user to the login scene upon clicking the login button
        SceneManager.LoadScene("Login");
    }
}
