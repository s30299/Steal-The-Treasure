using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] public SceneAsset firstLevel;
    [SerializeField] public SceneAsset mainMenu;
    public void Start()
    {
        if (PlayerPrefs.HasKey("posX"))
        {
            GameObject.FindGameObjectWithTag("Player").
                transform.SetPositionAndRotation(new(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), 0),
                Quaternion.identity);
        }
    }
    public static void ChangeLevel(string levelName)
    {
        InputManager.DisableInput();
        PlayerPrefs.SetString("currentLevel", levelName);
        SceneManager.LoadScene(levelName);
        PlayerPrefs.Save();
    }

    public void LoadFirstLevel()
    {
        ChangeLevel(firstLevel.name);
    }
    public static bool hasSavedLevel()
    {
        return PlayerPrefs.HasKey("currentLevel") && PlayerPrefs.GetString("currentLevel") != "";
    }
    public static void LoadCurrentLevel()
    {
        Time.timeScale = 1.0f;
        ChangeLevel(PlayerPrefs.GetString("currentLevel"));
    }

    public static void CleanSavedLevel()
    {
        PlayerPrefs.DeleteKey("currentLevel");
    }
    public void GoToMainMenu()
    {
        InputManager.DisableInput();
        SceneManager.LoadScene(mainMenu.name);
    }
}
