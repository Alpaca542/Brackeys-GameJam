using System.Collections;
using System.Collections.Generic;
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
    public void PlaySound(int whichsound, float pitchrangeDOWN, float pitchrangeUP)
    {
        GameObject gmb = Instantiate(audioHandler, transform.position, Quaternion.identity);
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

    public void StopPlaying()
    {
        sound.loop = false;
    }

    public void StartPlaying()
    {
        sound.loop = true;
        if (!sound.isPlaying)
        {
            sound.Play();
        }
        CancelInvoke(nameof(alterPitch));
        InvokeRepeating(nameof(alterPitch), 0f, sound.clip.length);
    }
    public void alterPitch()
    {
        sound.GetComponent<AudioSource>().pitch = Random.Range(pitchrangeDOWN, pitchrangeUP);
    }
}