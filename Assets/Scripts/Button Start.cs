using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("start button is clicked");
        SceneManager.LoadScene("Customization");
    }

    // public void OpenSettings()
    // {
    //     SceneManager.LoadScene("SettingsScene");
    // }

    // public void ExitGame()
    // {
    //     Application.Quit();

    //     #if UNITY_EDITOR
    //     UnityEditor.EditorApplication.isPlaying = false;
    //     #endif
    // }
}