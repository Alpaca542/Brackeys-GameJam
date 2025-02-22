using UnityEngine;
using System.Collections;
public class cloudscript : MonoBehaviour
{
    [Header("setting movenment")]
    [SerializeField] private float moveSpeedMax = 0.5f;
    [SerializeField] private float moveRadius = 0.5f;
    public Vector2 startPos;
    public Vector2 endPos;
    public bool shouldFlick = true;

    [Header("setting flicker")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float flickerDelayMin = 0.1f;
    [SerializeField] private float flickerDelayMax = 0.5f;
    [SerializeField] private float flickerSpeedMax = 1f;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float targetalpha;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position - new Vector3(6f, 6f, 0f);
        endPos = transform.position + new Vector3(6f, 6f, 0f);

        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("FirefliesFlicker needing component SpriteRenderer.");
            enabled = false;
        }

        StartCoroutine(UpdateMovement());
        if (shouldFlick)
        {
            StartCoroutine(UpdateFlicker());
        }
    }

    IEnumerator UpdateMovement()
    {
        while (true)
        {
            targetPosition = new Vector2(Random.Range(startPos.x, endPos.x), Random.Range(startPos.y, endPos.y));

            float step = 0f;
            float moveSpeed = Random.Range(0f, moveSpeedMax);
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(transform.position, targetPosition);
            Vector2 startpos = transform.position;
            float fractionOfJourney = 0;

            while (fractionOfJourney < 1f)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                fractionOfJourney = distCovered / journeyLength;
                step += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startpos, targetPosition, fractionOfJourney);
                yield return null;
            }
        }
    }


    IEnumerator UpdateFlicker()
    {
        while (true)
        {
            targetalpha = Random.Range(0f, 1f);
            float myalpha = spriteRenderer.color.a;
            float step = 0f;
            float flickerSpeed = Random.Range(0.1f, flickerSpeedMax);
            float startTime = Time.time;
            float journeyLength = Mathf.Abs(targetalpha - myalpha);
            float fractionOfJourney = 0;

            while (fractionOfJourney < 1f)
            {
                float distCovered = (Time.time - startTime) * flickerSpeed;
                fractionOfJourney = distCovered / journeyLength;
                step += Time.deltaTime * flickerSpeed;
                spriteRenderer.color = new Color32(255, 255, 255, (byte)(Mathf.Lerp(myalpha, targetalpha, fractionOfJourney) * 255));
                yield return null;
            }
        }
    }
}