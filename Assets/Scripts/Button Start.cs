using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuButtons : MonoBehaviour
{
    public GameObject transition;
    public void StartGame()
    {
        Debug.Log("start button is clicked");
        transition.transform.DOMove(new Vector3(0, 0, 0), 4f);
        GameObject.FindGameObjectWithTag("musicManager").GetComponent<musicscript>().Down();
        Invoke(nameof(lscene), 4f);
    }
    public void lscene()
    {
        SceneManager.LoadScene("GameScene");
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

    public void OnHoverOverMe()
    {
        GetComponent<RectTransform>().DOScale(new Vector3(1.06f, 1.06f, 1f), 0.5f);
    }
    void Start()
    {
        GameObject.FindGameObjectWithTag("musicManager").GetComponent<musicscript>().Up();

    }
    public void OnUnHoverOverMe()
    {
        GetComponent<RectTransform>().DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
    }
}