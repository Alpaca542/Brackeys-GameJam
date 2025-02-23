using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashBar : MonoBehaviour
{
    public Image fill;
    public float max;
    public GameObject playerLight;
    private IEnumerator crtn;
    public float shakeAmount = 0.2f;
    public float shakeSpeed = 10f;
    private Vector3 originalPosition;
    public float rate = 2f;
    public float curHealth;

    public bool decreasing;
    public float regenSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            Debug.Log(1);
            DecreaseStart();
        }
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            Debug.Log(2);
            DecreaseStop();
        }
        if (!decreasing)
        {
            if (curHealth < max)
            {
                curHealth += Time.deltaTime * regenSpeed;
                SetHealth(curHealth);
            }
            else
            {
                curHealth = max;
            }
        }
    }

    public void SetHealth(float health)
    {
        fill.GetComponent<RectTransform>().localScale = new Vector2((health / max), 1);

    }

    void Start()
    {
        curHealth = 100f;
    }

    public void DecreaseStart()
    {
        crtn = Shake();
        StartCoroutine(crtn);
        decreasing = true;
    }

    public void DecreaseStop()
    {
        StopCoroutine(crtn);
        playerLight.GetComponent<Playerlight>().StopShining();
        GetComponent<RectTransform>().localPosition = originalPosition;
        decreasing = false;
    }

    public IEnumerator Shake()
    {
        playerLight.GetComponent<Playerlight>().StartShining();
        originalPosition = GetComponent<RectTransform>().localPosition;
        while (curHealth >= 0)
        {
            curHealth -= rate * Time.deltaTime;
            SetHealth(curHealth);
            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1,
                Mathf.PerlinNoise(0, Time.time * shakeSpeed) * 2 - 1,
                0) * shakeAmount;
            GetComponent<RectTransform>().localPosition = originalPosition + offset;
            yield return null;
        }
        curHealth = 0;
        playerLight.GetComponent<Playerlight>().StopShining();
        GetComponent<RectTransform>().localPosition = originalPosition;
    }
}