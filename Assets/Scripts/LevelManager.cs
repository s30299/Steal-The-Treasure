using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] public SceneAsset firstLevel;
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
        PlayerPrefs.SetString("currentLevel", levelName);
        SceneManager.LoadScene(levelName);
        PlayerPrefs.Save();
    }

    public void LoadFirstLevel()
    {
        ChangeLevel(firstLevel.name);
    }
}
