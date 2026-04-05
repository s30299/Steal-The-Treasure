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
        AbilityCheck();
        SetCode();
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

        PlayerPrefs.SetInt("Code1", Random.Range(0, 10));
        PlayerPrefs.SetInt("Code2", Random.Range(0, 10));
        PlayerPrefs.SetInt("Code3", Random.Range(0, 10));

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

    public static void ReloadScene()
    {
        Time.timeScale = 1.0f;
        UIManager.DisableGameOverUI();
        InputManager.DisableInput();
        SceneManager.LoadScene(PlayerPrefs.GetString("currentLevel", "Treasury"));
    }

    private void AbilityCheck()
    {
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
    private void SetCode()
    {
        var code1 = GameObject.FindGameObjectWithTag("Code1");
        var code2 = GameObject.FindGameObjectWithTag("Code2");
        var code3 = GameObject.FindGameObjectWithTag("Code3");

        if (code1 != null)
        {
            code1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Code/" + PlayerPrefs.GetInt("Code1").ToString());
        }
        else if (code2 != null)
        {
            code2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Code/" + PlayerPrefs.GetInt("Code2").ToString());
        }
        else if(code3 != null)
        {
            code3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Code/" + PlayerPrefs.GetInt("Code3").ToString());
        }
    }
}
