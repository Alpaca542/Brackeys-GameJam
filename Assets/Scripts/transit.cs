using UnityEngine;
using DG.Tweening;

public class transit : MonoBehaviour
{
    public GameObject transition;
    void Start()
    {
        transition.transform.DOMove(new Vector3(68f, 2.2f, 0), 4f);
    }
}
