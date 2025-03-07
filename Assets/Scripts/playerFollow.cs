using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class playerFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private Vector3 playerVector;
    public GameObject myUI;
    public int speed;
    public GameObject transition;
    public musicscript musicManager;
    void Start()
    {
        transition.transform.DOLocalMove(new Vector3(58f, 2.2f, 10f), 4f);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        playerVector = player.position;
        playerVector.z = -10;
        transform.position = Vector3.Lerp(transform.position, playerVector, speed * Time.deltaTime);
    }
    public void Lose()
    {
        transition.transform.DOLocalMove(new Vector3(0f, 0f, 10f), 4f);
        GameObject.FindGameObjectWithTag("musicManager").GetComponent<musicscript>().Down();
        Invoke(nameof(SceneTransLose), 4f);
    }
    public void Win()
    {
        transition.transform.DOLocalMove(new Vector3(0f, 0f, 10f), 4f);
        GameObject.FindGameObjectWithTag("musicManager").GetComponent<musicscript>().Down();
        Invoke(nameof(SceneTransWin), 4f);
    }

    public void SceneTransLose()
    {
        SceneManager.LoadScene("LoseScene");
    }

    public void SceneTransWin()
    {
        SceneManager.LoadScene("EndScene");
    }
}
