using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public SpriteRenderer mySpriteRen;
    public Sprite[] spriteSteps;
    public int curStep;
    private MemoryManager memManager;
    private DialogueScript dialogueManager;

    void Start()
    {
        memManager = GameObject.FindGameObjectWithTag("MemManager").GetComponent<MemoryManager>();
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueMngr").GetComponent<DialogueScript>();
    }
    void Update()
    {
        if (memManager.step != curStep)
        {
            curStep = memManager.step;
            mySpriteRen.sprite = spriteSteps[curStep];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueManager.StartMainLine();
    }
}
