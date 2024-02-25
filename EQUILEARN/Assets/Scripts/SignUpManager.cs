using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using Firebase.Extensions;

public class SignUpManager : MonoBehaviour
{
    // UI elements for user input and interaction
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public Toggle termsToggle;
    public Button createAccountButton;
    public Button loginButton;
    public TextMeshProUGUI feedbackText; // Displays messages to the user

    private Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"); // Regular expression to validate email format
    private FirebaseAuth auth; // Reference to the Firebase Auth object

    public static string UserEmail { get; private set; } // Static property to store user email across scenes

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance; // Initialize Firebase Auth

        // Attach event listeners to buttons
        createAccountButton.onClick.AddListener(OnCreateAccountClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);

        // Configure input fields to securely handle password input
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        confirmPasswordInputField.contentType = TMP_InputField.ContentType.Password;
    }

    private void OnCreateAccountClicked()
    {
        // Retrieve user input from UI elements
        string name = nameInputField.text;
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // Validate user input and provide feedback
        if (!termsToggle.isOn)
        {
            UpdateFeedback("You must agree to the Terms & Conditions.");
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            UpdateFeedback("Name is required.");
            return;
        }

        if (!IsEmailValid(email))
        {
            UpdateFeedback("Enter a valid email.");
            return;
        }

        if (password != confirmPassword)
        {
            UpdateFeedback("Passwords do not match.");
            return;
        }

        if (!IsPasswordValid(password))
        {
            UpdateFeedback("Password does not meet requirements.");
            return;
        }

        // Attempt to create a new user with Firebase Auth
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                UpdateFeedback("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                UpdateFeedback("Error during account creation: " + task.Exception.Message);
                return;
            }

            // Successfully created a new user, proceed with email verification
            FirebaseUser newUser = task.Result.User;
            newUser.SendEmailVerificationAsync().ContinueWithOnMainThread(verifyTask => {
                if (verifyTask.IsCanceled || verifyTask.IsFaulted)
                {
                    UpdateFeedback("Error sending verification email: " + verifyTask.Exception.Message);
                }
                else
                {
                    UserEmail = newUser.Email; // Store user email
                    SceneManager.LoadScene("EmailVerification"); // Navigate to the email verification scene
                }
            });
        });
    }

    private void OnLoginButtonClicked()
    {
        SceneManager.LoadScene("Login"); // Navigate to the login scene
    }

    private bool IsEmailValid(string email)
    {
        // Validate email using regex
        return emailRegex.IsMatch(email);
    }

    private bool IsPasswordValid(string password)
    {
        // Check password length for validity
        return password.Length >= 8;
    }

    private void UpdateFeedback(string message)
    {
        // Display feedback to the user
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
        else
        {
            Debug.LogError("Feedback text component not set in the inspector.");
        }
    }
}
