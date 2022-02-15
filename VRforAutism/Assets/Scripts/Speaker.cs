using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    [SerializeField] private float minutesToClose;
    [SerializeField] private AudioClip audioRemainingMinutes15;
    [SerializeField] private AudioClip audioRemainingMinutes10;
    [SerializeField] private AudioClip audioRemainingMinutes5;
    [SerializeField] private AudioClip audioSupermarketClose;
    [SerializeField] private AudioClip audioAnnounce;
    [SerializeField] private AudioSource audioSource;

    private const float FirstAnnounceTime = 900.0f; //15 minuti
    private const float SecondAnnounceTime = 600.0f; //10 minuti
    private const float ThirdAnnounceTime = 300.0f; //5 minuti
    private const float BellSoundTime = 3.0f;
    
    public void Start()
    {
        StartCoroutine(StartWaiting(audioRemainingMinutes15, minutesToClose*60.0f - FirstAnnounceTime));
        StartCoroutine(StartWaiting(audioRemainingMinutes10, minutesToClose*60.0f - SecondAnnounceTime));
        StartCoroutine(StartWaiting(audioRemainingMinutes5, minutesToClose*60.0f - ThirdAnnounceTime));
        StartCoroutine(StartWaiting(audioSupermarketClose, minutesToClose*60.0f));
    }

    private void PlayAnnounce(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private IEnumerator StartWaiting(AudioClip audioClip, float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        PlayAnnounce(audioAnnounce);
        yield return new WaitForSeconds(BellSoundTime);
        PlayAnnounce(audioClip);
    }
}