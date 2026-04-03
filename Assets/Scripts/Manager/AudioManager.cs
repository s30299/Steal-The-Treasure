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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        musicSource = GetComponent<AudioSource>();
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
            playerController = player.GetComponent<PlayerController>();
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
        PlayerPrefs.SetFloat("effectsVolume", volume);
        PlayerPrefs.Save();
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
        if (Time.timeScale == 0)
        {
            playerAudioSource.volume = 0;
        }
        else
        {
            playerAudioSource.volume = PlayerPrefs.GetFloat("effectsVolume", 1);
        }

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
}
