using UnityEngine;

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
}
