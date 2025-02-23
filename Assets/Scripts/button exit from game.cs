using UnityEngine;
using DG.Tweening;

public class buttonexitfromgame : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("exit from game button is clicked");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnHoverOverMe()
    {
        GetComponent<RectTransform>().DOScale(new Vector3(1.06f, 1.06f, 1f), 0.5f);
    }

    public void OnUnHoverOverMe()
    {
        GetComponent<RectTransform>().DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
    }
}
