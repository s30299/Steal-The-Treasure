using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject gameOverUI;
    private Canvas gameOverCanvas;
    [SerializeField] private GameObject pauseMenuUI;
    private Canvas pauseMenuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        gameOverCanvas = gameOverUI.GetComponent<Canvas>();
        pauseMenuCanvas = pauseMenuUI.GetComponent<Canvas>();
    }
    public void EnableGameOverUI()
    {
        gameOverCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("ReviveButton"));
    }
    public void DisableGameOverUI() {
        gameOverCanvas.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        DisableGameOverUI();
        FindAnyObjectByType<InputManager>().DisableInput();
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    public void Pause()
    {
        Time.timeScale=0;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("ContinueButton"));
        pauseMenuCanvas.enabled = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1.0f;
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuCanvas.enabled=false;
    }
    public void SwitchPause()
    {
        if (pauseMenuCanvas.enabled)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
