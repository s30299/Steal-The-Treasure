using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource musicSource;
    private List<AudioSource> audioSources=new();
    public static float effectsVolume=1;
    public static float musicVolume=1;
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        foreach (AudioSource source in FindObjectsByType<AudioSource>()) 
        {
            if (source != musicSource)
            {
                audioSources.Add(source);
            }
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicVolume = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        //audioSources.ForEach(source => source.volume = volume);
        effectsVolume = volume;
    }
}
