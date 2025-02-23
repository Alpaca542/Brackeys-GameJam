using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    private AudioSource sound;
    public AudioClip[] soundslist;
    public bool shouldILoop = false;
    public bool amIPlaying = false;
    public bool shouldIPlayOnStart = false;
    public GameObject audioHandler;
    public float pitchrangeDOWN;
    public float pitchrangeUP;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    public void PlaySound(int whichsound, float pitchrangeDOWN, float pitchrangeUP, bool spawnIn)
    {
        GameObject gmb;
        if (spawnIn)
        {
            gmb = Instantiate(audioHandler, transform.position, Quaternion.identity, transform);
        }
        else
        {
            gmb = Instantiate(audioHandler, transform.position, Quaternion.identity);
        }
        gmb.GetComponent<AudioSource>().clip = soundslist[whichsound];
        gmb.GetComponent<AudioSource>().pitch = Random.Range(pitchrangeDOWN, pitchrangeUP);
        gmb.GetComponent<AudioSource>().Play();
    }
    public void PlaySoundSecond(int whichsound, float pitchrangeDOWN, float pitchrangeUP, float volume)
    {
        GameObject gmb = Instantiate(audioHandler, transform.position, Quaternion.identity);
        gmb.GetComponent<AudioSource>().clip = soundslist[whichsound];
        gmb.GetComponent<AudioSource>().pitch = Random.Range(pitchrangeDOWN, pitchrangeUP);
        gmb.GetComponent<AudioSource>().volume = volume;
        gmb.GetComponent<AudioSource>().Play();
    }

    public void StopPlaying(bool fade)
    {
        if (fade)
        {
            sound.DOFade(0f, 1f);
        }
        else
        {
            sound.loop = false;
        }
    }

    public void StartPlaying(bool fade)
    {
        if (fade)
        {
            sound.DOFade(0.3f, 1f);
        }
        else
        {
            sound.loop = true;
            if (!sound.isPlaying)
            {
                sound.Play();
            }
            CancelInvoke(nameof(alterPitch));
            InvokeRepeating(nameof(alterPitch), 0f, sound.clip.length);
        }
    }
    public void alterPitch()
    {
        sound.GetComponent<AudioSource>().pitch = Random.Range(pitchrangeDOWN, pitchrangeUP);
    }
}