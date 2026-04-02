using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private string itemName = "Item";

    [Header("Progress")]
    [SerializeField] private PlayerProgress progress;

    [Header("Skill")]
    [SerializeField] private SkillDefinition skillDefinition;
    [SerializeField] private int levelGain = 1;
    [SerializeField] private int maxLevel = 999;

    public void Collect()
    {
        if (progress == null)
        {
            Debug.LogWarning($"Collectible '{name}' has no PlayerProgress assigned.");
            return;
        }

        if (skillDefinition == null)
        {
            Debug.LogWarning($"Collectible '{name}' has no SkillDefinition assigned.");
            return;
        }

        SkillState state = progress.skills.Find(s => s != null && s.skillDefinition == skillDefinition);

        if (state == null)
        {
            state = new SkillState
            {
                skillDefinition = skillDefinition,
                currentLevel = 1
            };

            progress.skills.Add(state);
        }

        int oldLevel = state.currentLevel;
        state.currentLevel = Mathf.Clamp(state.currentLevel + levelGain, 1, maxLevel);

        Debug.Log($"Collected: {itemName} | {skillDefinition.name}: {oldLevel} -> {state.currentLevel}");
        if (skillDefinition.id == "dash")
        {
            PlayerPrefs.SetInt("dashCollected", 1);
            PlayerPrefs.Save();
        }
        else if (skillDefinition.id == "jump")
        {
            PlayerPrefs.SetInt("doubleJumpCollected", 1);
            PlayerPrefs.Save();
        }
            Destroy(gameObject);
    }
}