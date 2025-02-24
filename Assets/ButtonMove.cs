using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
public class ButtonMove : MonoBehaviour
{
    public GameObject myImg;
    void OnEnable()
    {
        StartCoroutine(Fluctuate());
    }
    public IEnumerator Fluctuate()
    {
        myImg.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        myImg.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 255), 1f).SetUpdate(true);
        myImg.GetComponent<RectTransform>().DOLocalMoveX(43f, 0.6f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.6f);
        while (true)
        {
            myImg.GetComponent<RectTransform>().DOLocalMoveX(90f, 1.2f).SetUpdate(true).SetEase(Ease.Linear);
            yield return new WaitForSecondsRealtime(1.2f);
            myImg.GetComponent<RectTransform>().DOLocalMoveX(43f, 1.2f).SetUpdate(true).SetEase(Ease.Linear);
            yield return new WaitForSecondsRealtime(1.2f);
        }
    }
}
