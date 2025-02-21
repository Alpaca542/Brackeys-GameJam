using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBar : MonoBehaviour
{
    public Image fill;
    public float max;
    private IEnumerator crtn;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;
    private Vector3 originalPosition;

    public void SetMaxHealth(int health)
    {

    }

    public void SetHealth(float health)
    {
        fill.GetComponent<RectTransform>().sizeDelta = new Vector2((health / max) * 990, fill.GetComponent<RectTransform>().sizeDelta.y);

    }

    void Start()
    {
        ShakeStart();
        SetHealth(10f);
    }

    public void ShakeStart()
    {
        crtn = Shake();
        StartCoroutine(crtn);
    }

    public void ShakeStop()
    {
        StopCoroutine(crtn);
    }

    public IEnumerator Shake()
    {
        originalPosition = GetComponent<RectTransform>().position;
        while (true)
        {
            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1,
                Mathf.PerlinNoise(0, Time.time * shakeSpeed) * 2 - 1,
                0) * shakeAmount;
            GetComponent<RectTransform>().position = originalPosition + offset;
            yield return null;
        }
    }

}