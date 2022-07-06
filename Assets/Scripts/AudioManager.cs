using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            
        }
    }

    void Start()
    {
        Play("BackgroundMusic");
    }

    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("\"" + name + "\" SOUND NOT FOUND");
        }
            
        
        s.source.Play();
    }
}
