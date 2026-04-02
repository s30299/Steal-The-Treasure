using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] public SceneAsset firstLevel;
    [SerializeField] public SceneAsset mainMenu;
    public void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (PlayerPrefs.HasKey("posX")&& player!=null)
        {
            player.transform.SetPositionAndRotation(new(PlayerPrefs.GetFloat("posX"),
                PlayerPrefs.GetFloat("posY"), 0),
                Quaternion.identity);
        }
        else if(player != null)
        {
            PlayerPrefs.SetFloat("posY",player.transform.position.y);
            PlayerPrefs.SetFloat("posX", player.transform.position.x);
            PlayerPrefs.Save();
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
        PlayerPrefs.DeleteKey("posX");
        PlayerPrefs.DeleteKey("posY");
        PlayerPrefs.Save();
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
