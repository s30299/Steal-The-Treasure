using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoSingleton<LevelManager>
{
    //[SerializeField] private SceneAsset firstLevel;
    //[SerializeField] private SceneAsset mainMenu;
    [SerializeField] private PlayerProgress playerProgress;
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

        if (PlayerPrefs.HasKey("dashCollected"))
        {
            var collectible = GameObject.Find("Dash_Collectible");
            if (collectible != null) { Destroy(collectible); }
            playerProgress.skills[1].currentLevel = 1;
        }
        else
        {
            playerProgress.skills[1].currentLevel = 0;
        }

        if (PlayerPrefs.HasKey("doubleJumpCollected"))
        {
            var collectible = GameObject.Find("DoubleJump_Collectible");
            if (collectible != null) { Destroy(collectible); }
            playerProgress.skills[0].currentLevel = 2;
        }
        else
        {
            playerProgress.skills[0].currentLevel = 1;
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
        PlayerPrefs.DeleteKey("dashCollected");
        PlayerPrefs.DeleteKey("doubleJumpCollected");
        PlayerPrefs.Save();
        ChangeLevel("Treasury");
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
        SceneManager.LoadScene("MainMenu");
    }
}
