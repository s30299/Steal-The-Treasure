using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDefinition", menuName = "Skill/SkillDefinition")]
public class SkillDefinition : ScriptableObject
{
    public string id;
    public SkillType skillType;
    public int maxLevel;
}