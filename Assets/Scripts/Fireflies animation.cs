using UnityEngine;
using System.Collections;
public class FirefliesFlicker : MonoBehaviour
{
    [Header("setting movenment")]
    [SerializeField] private float moveSpeedMax = 1f;
    [SerializeField] private float minPauseTime = 0.5f;
    [SerializeField] private float maxPauseTime = 2f;
    [SerializeField] private float moveRadius = 0.5f;
    public Transform startPos;
    public Transform endPos;
    public bool shouldFlick;

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
            targetPosition = new Vector2(Random.Range(startPos.position.x, endPos.position.x), Random.Range(startPos.position.y, endPos.position.y));

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
            float pauseTime = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseTime);
    }
}


    IEnumerator UpdateFlicker()
    {
        while (true)
        {            
            targetalpha = Random.Range(0f, 1f);
            float myalpha = GetComponent<SpriteRenderer>().color.a;
            float step = 0f;
            float flickerSpeed = Random.Range(0.1f, flickerSpeedMax);
            float startTime = Time.time;
            float journeyLength = Mathf.Abs(targetalpha-myalpha);
            float fractionOfJourney = 0;

            while (fractionOfJourney < 1f)
            {
                float distCovered = (Time.time - startTime) * flickerSpeed;
                fractionOfJourney = distCovered / journeyLength;
                step += Time.deltaTime * flickerSpeed;
                GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte) (Mathf.Lerp(myalpha, targetalpha, fractionOfJourney)*255));
                yield return null;
            }
            float pauseTime = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseTime);
        }
    }
}