using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("UI/Debug")]
    [SerializeField] private string _itemName = "Item";

    [Header("Progress")]
    [SerializeField] private PlayerProgress _progress;

    [Header("What skill to level up")]
    [SerializeField] private DashSkillDefinition _dashDefinition;

    [SerializeField] private int _levelGain = 1;
    [SerializeField] private int _maxLevel = 999;

    public void Collect()
    {
        if (_progress == null || _dashDefinition == null)
        {
            Debug.LogWarning($"Collectible '{name}': Missing references (_progress / _dashDefinition).");
            return;
        }

        var state = _progress.skills.Find(s => s != null && s.skillDefinition == _dashDefinition);

        if (state == null)
        {
            state = new SkillState
            {
                skillDefinition = _dashDefinition,
                currentLevel = 1
            };
            _progress.skills.Add(state);
        }

        int before = state.currentLevel;
        state.currentLevel = Mathf.Clamp(state.currentLevel + _levelGain, 1, _maxLevel);

        Debug.Log($"Interaction: Collected {_itemName} | Dash lvl {before} -> {state.currentLevel}");

        Destroy(gameObject);
    }
}