using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    // Duration of the splash screen display in seconds
    public float splashDuration = 3.0f;

    // Name of the login scene
    public string LoginSceneName = "Login";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TransitionToLogin());
    }

    IEnumerator TransitionToLogin()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(splashDuration);

        // After the wait, load the login scene
        SceneManager.LoadScene(LoginSceneName);
    }
}


