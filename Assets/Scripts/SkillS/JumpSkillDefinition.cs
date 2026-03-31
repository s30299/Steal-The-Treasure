using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpSkillDefinition", menuName = "Skill/JumpSkillDefinition")]
public class JumpSkillDefinition : SkillDefinition
{
    [Header("Jump Settings")]
    [SerializeField, Range(0f, 10f)]
    private float[] _jumpHeightPerLevel;

    [SerializeField, Range(0, 5)]
    private int[] _maxAirJumpsPerLevel;

    [SerializeField, Range(0f, 8f)]
    private float[] _downwardMovementMultiplierPerLevel;

    [SerializeField, Range(0f, 8f)]
    private float[] _upwardMovementMultiplierPerLevel;

    [SerializeField, Range(0f, 0.3f)]
    private float[] _coyoteTimePerLevel;

    [SerializeField, Range(0f, 0.3f)]
    private float[] _jumpBufferTimePerLevel;

    private int Clamp(int lvl, Array array)
    {
        return Mathf.Clamp(lvl - 1, 0, array.Length - 1);
    }

    public float GetJumpHeight(int level) => _jumpHeightPerLevel[Clamp(level, _jumpHeightPerLevel)];
    public int GetMaxAirJumps(int level) => _maxAirJumpsPerLevel[Clamp(level, _maxAirJumpsPerLevel)];
    public float GetDownwardMovementMultiplier(int level) => _downwardMovementMultiplierPerLevel[Clamp(level, _downwardMovementMultiplierPerLevel)];
    public float GetUpwardMovementMultiplier(int level) => _upwardMovementMultiplierPerLevel[Clamp(level, _upwardMovementMultiplierPerLevel)];
    public float GetCoyoteTime(int level) => _coyoteTimePerLevel[Clamp(level, _coyoteTimePerLevel)];
    public float GetJumpBufferTime(int level) => _jumpBufferTimePerLevel[Clamp(level, _jumpBufferTimePerLevel)];
}