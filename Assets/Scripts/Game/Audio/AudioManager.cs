using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    public enum SoundType
    {
        Jab,
        Damage
    }
    [System.Serializable]
    public class Sound
    {
        public SoundType soundType;
        public AudioClip audioClip;
        [Range(0f,1f)]
        public float Volume=1f;
        [HideInInspector]
        public AudioSource audioSource;


    }
    // allow other scripts to reference this script
    public static AudioManager Instance;

    //all sounds and their descriptions
    public Sound[] allSounds;

    //Dictionary of all sounds
    private Dictionary<SoundType, Sound> soundDictionary = new Dictionary<SoundType, Sound>();

    private void awake()
    {
        Instance = this;
        foreach(var s in allSounds)
        {
            soundDictionary[s.soundType] = s;
        }
    }

    public SoundType soundSelected;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(SoundType type)
    {
        if(!soundDictionary.TryGetValue(type, out Sound s))
        {
            return;
        }

        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        audioSrc.clip = s.audioClip;
        audioSrc.volume = s.Volume;

        audioSrc.Play();
        Destroy(soundObj, s.audioClip.length);
    }
}
