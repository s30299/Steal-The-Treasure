using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProgress", menuName = "Game/PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public List<SkillState> skills = new List<SkillState>();
}