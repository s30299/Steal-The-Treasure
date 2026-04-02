using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject gameOverUI;
    private Canvas gameOverCanvas;
    [SerializeField] private GameObject gameOverFirstButton;
    [SerializeField] private GameObject pauseMenuUI;
    private Canvas pauseMenuCanvas;
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject HUDUI;
    private static TextMeshProUGUI HUDTooltip;
    [SerializeField] private bool isInMainMenu = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        HUDTooltip = GameObject.Find("ToolTip (TMP)").GetComponent<TextMeshProUGUI>();
        HUDTooltip.enabled = false;
        gameOverCanvas = gameOverUI.GetComponent<Canvas>();
        pauseMenuCanvas = pauseMenuUI.GetComponent<Canvas>();
        //GameObject.Find("VolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("effectsVolume", 1);
        //GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume", 1);
    }
    public void EnableGameOverUI()
    {
        gameOverCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
    }
    public void DisableGameOverUI() {
        gameOverCanvas.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        DisableGameOverUI();
        InputManager.DisableInput();
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    public void Pause()
    {
        Time.timeScale=0;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        pauseMenuCanvas.enabled = true;
        GameObject.Find("VolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("effectsVolume",1);
        GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume",1);
    }
    public void Unpause()
    {
        Time.timeScale = 1.0f;
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuCanvas.enabled=false;
    }
    public void SwitchPause()
    {
        if (!gameOverCanvas.enabled && !isInMainMenu)
        {
            if (pauseMenuCanvas.enabled){Unpause();}
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
        HUDTooltip.enabled = false;
    }
}
