using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class TriggerScript : MonoBehaviour
{
    public SpriteRenderer mySpriteRen;
    public Sprite[] spriteSteps;
    public int curStep;
    private GameObject memManager;
    private DialogueScript dialogueManager;
    public bool amiforboss;

    void Start()
    {
        memManager = GameObject.FindGameObjectWithTag("MemManager");
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueMng").GetComponent<DialogueScript>();

        curStep = memManager.GetComponent<MemoryManager>().step;
        mySpriteRen.sprite = spriteSteps[curStep];
    }
    void Update()
    {
        if (memManager.GetComponent<MemoryManager>().step != curStep)
        {
            if (memManager.GetComponent<MemoryManager>().step >= 7 && !amiforboss)
            {
                Destroy(gameObject);
            }
            curStep = memManager.GetComponent<MemoryManager>().step;
            mySpriteRen.sprite = spriteSteps[curStep];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(StartMem());
        }
    }
    public IEnumerator StartMem()
    {
        Time.timeScale = 0f;
        Camera.main.DOOrthoSize(2f, 2f).SetUpdate(true);
        Camera.main.transform.DOMove(new Vector3(transform.position.x, transform.position.y, -10), 2f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(3f);
        if (amiforboss)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().jump = 14f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale = 1f;
            dialogueManager.IndexInMain = 14;
        }
        dialogueManager.StartMainLine();
        memManager.GetComponent<MemoryManager>().step++;
        Destroy(gameObject);
    }
}
