using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class manages the audio playing for an object
/// </summary>
public class AudioManager : MonoBehaviour
{   
    
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySoundFXClip(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        audioSource.clip = audioclip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
    }
}
