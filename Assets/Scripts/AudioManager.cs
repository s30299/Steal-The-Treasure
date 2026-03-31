using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource musicSource;
    private List<AudioSource> audioSources=new();
    
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
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume",1));
        SetEffectsVolume(PlayerPrefs.GetFloat("effectsVolume", 1));
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetEffectsVolume(float volume)
    {
        //audioSources.ForEach(source => source.volume = volume);
        PlayerPrefs.SetFloat("effectsVolume", volume);
        PlayerPrefs.Save();
    }
}
