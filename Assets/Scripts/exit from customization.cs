using UnityEngine;
using UnityEngine.SceneManagement;
public class exitfromcustomization : MonoBehaviour
{
    public void OnExitButtonClicked()
    {
        Debug.Log("exit from customozation is clicked");
        SceneManager.LoadScene("AnimationMainMenu");
    }
}
