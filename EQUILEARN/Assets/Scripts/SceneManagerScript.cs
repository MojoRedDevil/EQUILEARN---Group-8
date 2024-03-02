using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Adjust this variable to set the delay in seconds
    public float delayInSeconds = 5f;

    // Method to load the scene with a delay
    public void LoadSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delayInSeconds));
    }

    // Coroutine to load the scene after a delay
    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}

