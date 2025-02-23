using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicscript : MonoBehaviour
{
    public void Up()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        DOTween.To(() => GetComponent<AudioSource>().volume, x => GetComponent<AudioSource>().volume = x, 1f, 4f).SetUpdate(true);
    }
    public void Down()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        DOTween.To(() => GetComponent<AudioSource>().volume, x => GetComponent<AudioSource>().volume = x, 0f, 4f).SetUpdate(true);
    }
    void Start()
    {
        Up();
    }
}