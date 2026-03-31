using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject gameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EnableGameOverUI()
    {
        gameOverUI.GetComponent<Canvas>().enabled = true;
    }
    public void DisableGameOverUI() {
        gameOverUI.GetComponent<Canvas>().enabled = false;
    }
    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        DisableGameOverUI();
        FindAnyObjectByType<InputManager>().DisableInput();
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
