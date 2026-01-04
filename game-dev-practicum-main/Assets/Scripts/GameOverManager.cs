using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;

    [Header("Settings")]
    public float delayBeforeGameOver = 2f; // Wait for death animation to play

    void Start()
    {
        // Make sure game over panel is hidden at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Call this when player dies
    public void ShowGameOver()
    {
        // Wait a bit for death animation to play
        Invoke("DisplayGameOverPanel", delayBeforeGameOver);
    }

    void DisplayGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Pause the game (optional)
            Time.timeScale = 0f;
        }
    }

    // Connect this to the Main Menu button
    public void GoToMainMenu()
    {
        // Unpause the game before loading
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    // Optional: Restart level button
    public void RestartLevel()
    {
        // Unpause the game
        Time.timeScale = 1f;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}