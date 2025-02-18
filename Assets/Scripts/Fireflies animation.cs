using UnityEngine;
using System.Collections;
public class FirefliesFlicker : MonoBehaviour
{
    [Header("setting movenment")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float minPauseTime = 0.5f;
    [SerializeField] private float maxPauseTime = 2f;
    [SerializeField] private float moveRadius = 0.5f;

    [Header("setting flicker")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float flickerDelayMin = 0.1f;
    [SerializeField] private float flickerDelayMax = 0.5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
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
        StartCoroutine(UpdateFlicker());
    }

    IEnumerator UpdateMovement()
    {
        while (true)
        {
           
            float pauseTime = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseTime);

            targetPosition = startPosition + (Random.insideUnitSphere * moveRadius);
            targetPosition.z = 0;

            float step = 0f;
            while (step < 1f)
            {
                step += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPosition, step);
                yield return null;
            }
        }
    }

    IEnumerator UpdateFlicker()
    {
        while (true)
        {
            float alpha = Random.Range(minIntensity, maxIntensity);
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;

            float delay = Random.Range(flickerDelayMin, flickerDelayMax);
            yield return new WaitForSeconds(delay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, moveRadius);
    }
}