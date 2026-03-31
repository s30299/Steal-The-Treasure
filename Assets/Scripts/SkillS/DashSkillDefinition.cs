using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DashSkillDefinition", menuName = "Skill/DashSkillDefinition")]
public class DashSkillDefinition : SkillDefinition
{
    [Header("Dash Settings")]
    [SerializeField, Range(0f, 50f)]
    private float[] _dashSpeedPerLevel;

    [SerializeField, Range(0f, 1f)]
    private float[] _dashDurationPerLevel;
    [SerializeField, Range(0f, 1f)]
    private float[] _IFramesPerLevel;

    [Header("Dash Charges")]
    [SerializeField, Range(1, 10)]
    private int[] _maxDashesPerLevel;

    [SerializeField, Range(0f, 5f)]
    private float[] _dashRechargeTimePerLevel;

    // --- Safe Guard | array.length < maxLevel ---
    private int Clamp(int lvl, Array array)
    {
        return Mathf.Clamp(lvl - 1, 0, array.Length - 1);
    }
    public float GetDashSpeed(int level) => _dashSpeedPerLevel[Clamp(level, _dashSpeedPerLevel)];
    public float GetDashDuration(int level) => _dashDurationPerLevel[Clamp(level, _dashDurationPerLevel)];
    public float GetIFrames(int level) => _IFramesPerLevel[Clamp(level, _IFramesPerLevel)];
    public int GetMaxDashes(int level) => _maxDashesPerLevel[Clamp(level, _maxDashesPerLevel)];
    public float GetDashRechargeTime(int level) => _dashRechargeTimePerLevel[Clamp(level, _dashRechargeTimePerLevel)];

}