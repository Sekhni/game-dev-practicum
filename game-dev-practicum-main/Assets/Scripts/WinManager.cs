using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject victoryPanel;

    [Header("Settings")]
    public float delayBeforeVictory = 1f; // Small delay before showing victory screen

    private bool hasWon = false;

    void Start()
    {
        // Make sure victory panel is hidden at start
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Check if all enemies are defeated
        if (!hasWon)
        {
            CheckForVictory();
        }
    }

    void CheckForVictory()
    {
        // Find all enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // If no enemies left, player wins!
        if (enemies.Length == 0)
        {
            TriggerVictory();
        }
    }

    void TriggerVictory()
    {
        hasWon = true;
        Debug.Log("Victory! All enemies defeated!");

        // Wait a bit before showing victory screen
        Invoke("ShowVictoryScreen", delayBeforeVictory);
    }

    void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            // Optional: Pause the game
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

    // Optional: Next level button
    public void LoadNextLevel()
    {
        // Unpause the game
        Time.timeScale = 1f;

        // Load next level (you can change the scene name)
        SceneManager.LoadScene("Level2");
    }
}