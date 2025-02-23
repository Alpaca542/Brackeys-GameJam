using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class textappearend : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("appear", 4f);
    }
    public void appear()
    {
        GetComponent<Image>().DOColor(new Color32(255, 255, 255, 255), 3f);
    }
}
