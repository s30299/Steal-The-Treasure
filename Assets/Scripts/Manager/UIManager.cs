using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameOverFirstButton;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject HUDUI;

    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject winFirstButton;

    private static TextMeshProUGUI HUDTooltip;
    [SerializeField] private bool isInMainMenu = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        HUDTooltip = GameObject.Find("ToolTip (TMP)").GetComponent<TextMeshProUGUI>();
        HUDTooltip.enabled = false;
        InputManager.CaptureCursor();
        //GameObject.Find("VolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("effectsVolume", 1);
        //GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume", 1);
    }
    public void EnableGameOverUI()
    {
        gameOverUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
        InputManager.ShowCursor();
    }
    public static void DisableGameOverUI() {
        Instance.gameOverUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        InputManager.CaptureCursor();
    }
    
    public void Pause()
    {
        Time.timeScale=0;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        pauseMenuUI.SetActive(true);
        GameObject.Find("VolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("effectsVolume",1);
        GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume",1);
        InputManager.ShowCursor();
    }
    public void Unpause()
    {
        Time.timeScale = 1.0f;
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuUI.SetActive(false);
        InputManager.CaptureCursor();
    }
    public void SwitchPause()
    {
        if (!gameOverUI.activeInHierarchy && !isInMainMenu && !winUI.activeInHierarchy)
        {
            if (pauseMenuUI.activeInHierarchy){Unpause();}
            else{Pause();}
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public static void ShowTooltip(string text)
    {
        HUDTooltip.GetComponent<TextMeshProUGUI>().text = text;
        HUDTooltip.enabled = true;
    }
    public static void HideTooltip()
    {
        try{
            HUDTooltip.enabled = false;
        }
        catch{
            Debug.LogError("Failed to hide tooltip.");
        }
    }
    public static void ShowWinScreen()
    {
        Instance.winUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Instance.winFirstButton);
        InputManager.ShowCursor();
    }
}
