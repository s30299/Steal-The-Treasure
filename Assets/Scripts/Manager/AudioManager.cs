using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    None,Walking
}
public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource musicSource;
    private AudioSource playerAudioSource;
    private static PlayerState playerState=PlayerState.None;
    private PlayerController playerController;
    private double lastPlayedLandingSound = 0;
    private GameObject player;
    private void Awake()
    {
        base.Awake();
        musicSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
            playerController = player.GetComponent<PlayerController>();
        }
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume",1));
        SetEffectsVolume(PlayerPrefs.GetFloat("effectsVolume", 1));
        UnpauseSoundEffects();
    }
    private List<AudioSource> GetAudioSources()
    {
        List<AudioSource> sources = new();
        foreach(AudioSource source in FindObjectsByType<AudioSource>())
        {
            if (source != musicSource)
            {
                sources.Add(source);
            }
        }
        return sources;
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetEffectsVolume(float volume)
    {
        GetAudioSources().ForEach(source=>source.volume = volume);
        PlayerPrefs.SetFloat("effectsVolume", volume);
        PlayerPrefs.Save();
    }
    public static void PauseSoundEffects()
    {
        Instance.GetAudioSources().ForEach(source=>source.Pause());
    }
    public static void UnpauseSoundEffects()
    {
        Instance.GetAudioSources().ForEach(source => source.UnPause());
    }
    private void Update()
    {
        if (player != null)
        {
            PlayerAudio();
        }
    }
    private void PlayerAudio()
    {

        if (playerState == PlayerState.Walking)
        {
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.PlayOneShot(playerController.walkSound);
            }
        }
    }

    public static void PlayerWalking(bool b)
    {
        if (playerState == PlayerState.None && b)
        {
            playerState = PlayerState.Walking;
        }
        else if (playerState == PlayerState.Walking && !b)
        {
           playerState = PlayerState.None;
        }
    }
    public static void PlayerJumped()
    {
        Instance.playerAudioSource.Stop();
        Instance.playerAudioSource.PlayOneShot(Instance.playerController.JumpSound);
    }
    //public static void PlayerLanded()
    //{
    //    if (Time.timeAsDouble - Instance.lastPlayedLandingSound > 1)
    //    {
    //        Instance.playerAudioSource.Stop();
    //        Instance.playerAudioSource.PlayOneShot(Instance.playerController.JumpSound);
    //        Instance.lastPlayedLandingSound= Time.timeAsDouble;
    //    }
    //    playerState = PlayerState.None;
    //}
    public static void PlayerDashed()
    {
        Instance.playerAudioSource.Stop();
        Instance.playerAudioSource.PlayOneShot(Instance.playerController.dashSound);
    }

    public static AudioSource GetMusicSource()
    {
        return Instance.musicSource;
    }
}
