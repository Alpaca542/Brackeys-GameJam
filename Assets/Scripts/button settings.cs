using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsettings : MonoBehaviour
{
    // public void StartGame()
    // {
    //     Debug.Log("start button is clicked");
    //     SceneManager.LoadScene("GameScene");
    // }

    public void OpenSettings()
    {
        Debug.Log("setting button is clicked");
        SceneManager.LoadScene("SettingsScene");
    }

    // public void ExitGame()
    // {
    //     Application.Quit();

    //     #if UNITY_EDITOR
    //     UnityEditor.EditorApplication.isPlaying = false;
    //     #endif
    // }
}