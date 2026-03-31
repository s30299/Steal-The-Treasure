using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] public SceneAsset firstLevel;
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
