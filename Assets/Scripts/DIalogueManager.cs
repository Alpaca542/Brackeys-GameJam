using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public bool StopTime;
    public TMP_Text Display;
    public Image Display2;
    public string[] sentences;
    public bool ShouldIStopAfterpb;
    public bool noPlayer;
    public GameObject[] faces;
    public int[] stopindexes = { 7 };
    public int IndexInMain;
    public string Stringpb;
    public GameObject btnContinue;
    public GameObject cnv;
    public GameObject cnvInGame;
    public GameObject btnContinueFake;
    public float typingspeed = 0.02f;
    IEnumerator coroutine;
    public bool startImmediately;
    private void Start()
    {
        Time.timeScale = 1f;
        if (startImmediately)
        {
            coroutine = Type(sentences[IndexInMain], faces[IndexInMain], false);
            StartCoroutine(coroutine);
        }
        else
        {
            cnvInGame.SetActive(true);
            btnContinue.SetActive(false);
            //cnv.SetActive(false);
            IndexInMain = 0;
        }
    }
    public void StartCrtnRemotely(string WhatToType, GameObject WhatToShow, bool ShouldIStopAfter, float savedOrthoSize1)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Type(WhatToType, WhatToShow, ShouldIStopAfter);
        StartCoroutine(coroutine);
    }
    public void alldispoff()
    {
        foreach (GameObject gm in faces)
        {
            gm.SetActive(false);
        }
    }
    public IEnumerator Type(string WhatToType, GameObject WhatToShow, bool ShouldIStopAfter)
    {
        alldispoff();
        Debug.Log(1);
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
        ShouldIStopAfterpb = ShouldIStopAfter;
        Stringpb = WhatToType;
        if (!noPlayer)
        {
            if (Camera.main.GetComponent<playerFollow>().player.tag == "Player" && !startImmediately)
            {
                Camera.main.GetComponent<playerFollow>().player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }
        }
        cnv.SetActive(true);
        cnv.GetComponent<RectTransform>().DOLocalMoveY(0f, 1f).SetUpdate(true); ;
        cnvInGame.SetActive(false);
        btnContinue.SetActive(false);
        btnContinueFake.SetActive(false);
        Display.text = "";
        WhatToShow.SetActive(true);
        foreach (char letter1 in WhatToType.ToCharArray())
        {
            Display.text += letter1;
            GetComponent<AudioSource>().loop = true;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            if (letter1 == ".".ToCharArray()[0] || letter1 == "!".ToCharArray()[0] || letter1 == "?".ToCharArray()[0])
            {
                yield return new WaitForSecondsRealtime(0.8f);
                GetComponent<AudioSource>().loop = false;
            }
            else if (letter1 == ",".ToCharArray()[0])
            {
                yield return new WaitForSecondsRealtime(0.4f);
                GetComponent<AudioSource>().loop = false;
            }
            else if (letter1 == " ".ToCharArray()[0])
            {
                yield return new WaitForSecondsRealtime(0.08f);
                GetComponent<AudioSource>().loop = false;
            }
            else
            {
                yield return new WaitForSecondsRealtime(typingspeed);
            }
        }
        GetComponent<AudioSource>().loop = false;

        if (ShouldIStopAfter)
        {
            btnContinueFake.SetActive(true);
        }
        else
        {
            btnContinue.SetActive(true);
        }
    }
    public void StartMainLine()
    {
        coroutine = Type(sentences[IndexInMain], faces[IndexInMain], false);
        StartCoroutine(coroutine);
        Debug.Log(1);
    }
    public void ContinueTyping()
    {
        if (cnv.activeSelf)
        {
            IndexInMain++;
            if (Array.IndexOf(stopindexes, IndexInMain) == -1)
            {
                coroutine = Type(sentences[IndexInMain], faces[IndexInMain], false);
                StartCoroutine(coroutine);
            }
            else
            {
                if (noPlayer)
                {
                    cnvInGame.SetActive(true);
                    btnContinue.SetActive(false);
                    cnv.SetActive(false);
                }
                else
                {
                    Time.timeScale = 1f;
                    Camera.main.DOOrthoSize(4f, 2f).SetUpdate(true);

                    cnvInGame.SetActive(true);
                    btnContinue.SetActive(false);
                    cnv.GetComponent<RectTransform>().DOLocalMoveY(-1500f, 1f).SetUpdate(true); ;
                    Invoke(nameof(OffInv), 1f);

                    if (IndexInMain == stopindexes[0])
                    {
                        //
                    }
                }
            }
        }
    }

    public void StopTyping()
    {
        Time.timeScale = 1f;
        Camera.main.DOOrthoSize(4f, 2f).SetUpdate(true);
        cnvInGame.SetActive(true);
        btnContinue.SetActive(false);
        cnv.GetComponent<RectTransform>().DOLocalMoveY(-1500f, 1f).SetUpdate(true); ;
        Invoke(nameof(OffInv), 1f);
        Camera.main.transform.parent.GetComponent<playerFollow>().enabled = true;
    }
    public void OffInv()
    {
        cnv.SetActive(false);
    }
    // private void Update()
    // {
    //     if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && cnv.activeSelf)
    //     {
    //         StopCoroutine(coroutine);
    //         GetComponent<AudioSource>().loop = false;
    //         if (Display.text == Stringpb)
    //         {
    //             if (ShouldIStopAfterpb)
    //             {
    //                 StopTyping();
    //             }
    //             else
    //             {
    //                 ContinueTyping();
    //             }
    //         }
    //         else
    //         {
    //             if (ShouldIStopAfterpb)
    //             {
    //                 Display.text = Stringpb;
    //                 btnContinueFake.SetActive(true);
    //             }
    //             else
    //             {
    //                 Display.text = sentences[IndexInMain];
    //                 btnContinue.SetActive(true);
    //             }
    //         }
    //     }
    // }
}