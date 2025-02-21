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
    private float savedOrthoSize;
    public TMP_Text Display;
    public Image Display2;
    public string[] sentences;
    public bool ShouldIStopAfterpb;
    public bool noPlayer;
    public Animation startAnim;
    public AnimationClip startAnim2;
    public AnimationClip startAnim3;
    public AnimationClip startAnim4;
    public Sprite[] faces;
    public int[] stopindexes = { 7 };
    public int IndexInMain;
    public string Stringpb;
    public GameObject btnContinue;
    public GameObject cnv;
    public GameObject cnvInGame;
    public GameObject cnvInGame2;
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
            cnvInGame2.SetActive(true);
            btnContinue.SetActive(false);
            cnv.SetActive(false);
            IndexInMain = stopindexes[0];
        }
    }
    public void StartCrtnRemotely(string WhatToType, Sprite WhatToShow, bool ShouldIStopAfter, float savedOrthoSize1)
    {
        savedOrthoSize = savedOrthoSize1;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Type(WhatToType, WhatToShow, ShouldIStopAfter);
        StartCoroutine(coroutine);
    }
    public IEnumerator Type(string WhatToType, Sprite WhatToShow, bool ShouldIStopAfter)
    {
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
        if (StopTime)
        {
            Time.timeScale = 0f;
        }
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
        cnvInGame.SetActive(false);
        cnvInGame2.SetActive(false);
        btnContinue.SetActive(false);
        btnContinueFake.SetActive(false);
        Display.text = "";
        if (WhatToShow.name == "player")
        {
            Display2.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(420, 420);
        }
        else
        {
            Display2.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(221.65f, 0);
        }
        Display2.sprite = WhatToShow;
        foreach (char letter1 in WhatToType.ToCharArray())
        {
            Display.text += letter1;
            if (letter1 == ".".ToCharArray()[0] || letter1 == "!".ToCharArray()[0] || letter1 == "?".ToCharArray()[0])
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
            else if (letter1 == " ".ToCharArray()[0])
            {
                yield return new WaitForSecondsRealtime(0.05f);
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
    public void StartMainLine(float orthosize)
    {
        savedOrthoSize = orthosize;
        coroutine = Type(sentences[IndexInMain], faces[IndexInMain], false);
        StartCoroutine(coroutine);
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
                    if (IndexInMain == stopindexes[0])
                    {
                        startAnim.clip = startAnim2;
                        startAnim.Play();
                    }
                    else if (IndexInMain == stopindexes[1])
                    {
                        startAnim.clip = startAnim3;
                        startAnim.Play();
                    }
                    else if (IndexInMain == stopindexes[2])
                    {
                        startAnim.clip = startAnim4;
                        startAnim.Play();
                    }
                    cnvInGame.SetActive(true);
                    cnvInGame2.SetActive(true);
                    btnContinue.SetActive(false);
                    cnv.SetActive(false);
                    savedOrthoSize = 0;
                }
                else
                {
                    if (StopTime)
                    {
                        Time.timeScale = 1f;
                    }
                    cnvInGame.SetActive(true);
                    cnvInGame2.SetActive(true);
                    btnContinue.SetActive(false);
                    cnv.SetActive(false);
                    savedOrthoSize = 0;

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
        if (StopTime)
        {
            Time.timeScale = 1f;
        }
        cnvInGame.SetActive(true);
        cnvInGame2.SetActive(true);
        btnContinue.SetActive(false);
        cnv.SetActive(false);
        Camera.main.transform.parent.GetComponent<playerFollow>().enabled = true;
        Camera.main.DOOrthoSize(savedOrthoSize, 0.5f).SetUpdate(true);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && cnv.activeSelf)
        {
            //gameObject.GetComponent<soundManager>().sound.loop = false;
            //GetComponent<AudioSource>().Stop();
            StopCoroutine(coroutine);
            GetComponent<AudioSource>().loop = false;
            if (Display.text == Stringpb)
            {
                if (ShouldIStopAfterpb)
                {
                    StopTyping();
                }
                else
                {
                    ContinueTyping();
                }
            }
            else
            {
                if (ShouldIStopAfterpb)
                {
                    Display.text = Stringpb;
                    btnContinueFake.SetActive(true);
                }
                else
                {
                    Display.text = sentences[IndexInMain];
                    btnContinue.SetActive(true);
                }
            }
        }
    }
}