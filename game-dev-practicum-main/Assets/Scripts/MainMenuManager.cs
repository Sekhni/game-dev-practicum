using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Call this method when Play button is clicked
    public void PlayGame()
    {
        // Load your main game scene
        SceneManager.LoadScene("first sprit");
    }

    // Call this method when Settings button is clicked
    public void OpenSettings()
    {
        // Load your settings scene or open a settings panel
        // For now, this just prints to console
        Debug.Log("Settings button clicked - implement your settings here");

        // If you have a settings scene:
        // SceneManager.LoadScene("SettingsScene");

        // Or if you have a settings panel in this scene:
        // settingsPanel.SetActive(true);
    }

    // Call this method when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit button clicked");

        // This will quit the application
        Application.Quit();

        // Note: Application.Quit() doesn't work in the Unity Editor
        // To test quitting in the editor, you can add this:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}